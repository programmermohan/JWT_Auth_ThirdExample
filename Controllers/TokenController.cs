using JWT_Auth_ThirdExample.Auth;
using JWT_Auth_ThirdExample.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        readonly UserContext userContext;
        private readonly ILogHistory _logHistory;
        readonly ITokenService tokenService;
        public TokenController(UserContext userContext, ILogHistory logHistory, ITokenService tokenService)
        {
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this._logHistory = logHistory ?? throw new ArgumentNullException(nameof(logHistory));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string IP = HttpContext.Request.Host.Value;
            string accessToken = tokenModel.Access_Token;
            string refreshToken = tokenModel.Refresh_Token;

            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);

            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = userContext.loginHistories.FirstOrDefault(u => u.User.UserName == username && u.Access_Token == accessToken && u.RefreshToken == refreshToken);
            if (user == null || user.MacAdress != IP || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            bool IsLoggedIn = _logHistory.DifferentIPLoggedIn(IP, user.User, accessToken, refreshToken);
            if (IsLoggedIn)
                return Ok("User already loggedin in different device");

            var newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            long Id = _logHistory.UpdateRefreshToken(IP, user.User, newAccessToken, newRefreshToken);

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke(TokenModel tokenModel)
        {
            string IP = HttpContext.Request.Host.Value;
            var username = User.Identity.Name;

            var user = userContext.loginHistories.OrderByDescending(a=> a.AccessTokenExpiryTime).FirstOrDefault(u => u.User.UserName == username && u.MacAdress == IP);
            if (user == null) return BadRequest();

            long Id = _logHistory.UpdateRefreshToken(IP,user.User, tokenModel.Access_Token, tokenModel.Refresh_Token);

            return NoContent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
