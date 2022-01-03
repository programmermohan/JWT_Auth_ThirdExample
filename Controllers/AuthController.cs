using JWT_Auth_ThirdExample.Auth;
using JWT_Auth_ThirdExample.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserContext _userContext;
        private readonly ITokenService _tokenService;
        private readonly ILogHistory _logHistory;

        public AuthController(IConfiguration configuration, UserContext userContext, ITokenService tokenService, ILogHistory logHistory)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logHistory = logHistory ?? throw new ArgumentNullException(nameof(logHistory));
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] User UserModel)
        {
            if (UserModel == null)
            {
                return BadRequest("Invalid client request");
            }

            string IP = HttpContext.Request.Host.Value;
            var user = _userContext.Users.FirstOrDefault(u => (u.UserName == UserModel.UserName) && (u.Password == UserModel.Password));
            var role = _userContext.Users.Where(u => (u.UserName == UserModel.UserName) && (u.Password == UserModel.Password)).Select(a => a.Role).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, UserModel.UserName),
              new Claim(ClaimTypes.Role, role.RoleName)
            };

            bool IsAlreadyLoggedIn = _logHistory.UserLoggedInDifferentIP(IP, UserModel);
            if (IsAlreadyLoggedIn)
                return Ok("user is already loggedin");

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            //update the database login history
            long Id = _logHistory.UpdateLoginHistory(IP, user, accessToken, refreshToken);

            return Ok(new
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
