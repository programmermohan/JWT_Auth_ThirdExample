using JWT_Auth_ThirdExample.Auth;
using JWT_Auth_ThirdExample.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Data
{
    public partial class LogHistory : ILogHistory
    {
        private readonly UserContext _userContext;
        private readonly IConfiguration _configuration;

        public LogHistory(UserContext userContext, IConfiguration configuration)
        {
            _userContext = userContext;
            _configuration = configuration;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public long UpdateLoginHistory(string IP, User userModel, string AccessToken, string RefreshToken)
        {
            try
            {
                //disable other IPs and tokens for this user
                List<LoginHistory> lstloginHistory = _userContext.loginHistories.Where(a => a.User.UserName == userModel.UserName).ToList();
                lstloginHistory.ForEach(a =>
                {
                    a.IsActive = false; a.AccessToken_Revoke = true; a.RefreshToken_Revoke = true;
                });
                _userContext.SaveChanges();

                LoginHistory loginHistory = _userContext.loginHistories.FirstOrDefault(a => a.User.UserName == userModel.UserName && a.MacAdress == IP);
                if (loginHistory != null)
                {
                    loginHistory.MacAdress = IP;
                    loginHistory.Access_Token = AccessToken;
                    loginHistory.RefreshToken = RefreshToken;
                    loginHistory.IsActive = true;
                    loginHistory.RefreshToken_Revoke = false;
                    loginHistory.AccessToken_Revoke = false;
                    loginHistory.ModifyLogin = DateTime.Now;
                    loginHistory.AccessTokenExpiryTime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                    loginHistory.RefreshTokenExpiryTime = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

                    _userContext.SaveChanges();

                    return loginHistory.Id;
                }
                else
                {
                    loginHistory = new LoginHistory()
                    {
                        UserID = userModel.Id,
                        MacAdress = IP,
                        Access_Token = AccessToken,
                        RefreshToken = RefreshToken,
                        IsActive = true,
                        RefreshToken_Revoke = false,
                        AccessToken_Revoke = false,
                        LoginDate = DateTime.Now,
                        ModifyLogin = DateTime.Now,
                        AccessTokenExpiryTime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"])),
                        RefreshTokenExpiryTime = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]))
                    };

                    _userContext.loginHistories.Add(loginHistory);
                    _userContext.SaveChanges();

                    return loginHistory.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public bool UserLoggedInDifferentIP(string IP, User userModel)
        {
            try
            {
                bool IsUserLogin = false;

                LoginHistory loginHistory = _userContext.loginHistories.Where(a => a.User.UserName == userModel.UserName).OrderByDescending(a => a.AccessTokenExpiryTime).FirstOrDefault();

                if (loginHistory != null)
                {
                    if (loginHistory.AccessToken_Revoke == false)
                    {
                        TimeSpan timeDiff = DateTime.Now.Subtract(loginHistory.AccessTokenExpiryTime);

                        if (timeDiff.TotalMinutes < 20)
                        {
                            IsUserLogin = true;
                        }
                    }
                }

                return IsUserLogin;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public bool DifferentIPLoggedIn(string IP, User userModel, string AccessToken, string RefreshToken)
        {
            try
            {
                bool IsAnotherUserLogin = false;

                LoginHistory loginHistory = _userContext.loginHistories.
                    Where(a => a.User.UserName == userModel.UserName && a.Access_Token != AccessToken && a.RefreshToken != RefreshToken).
                    OrderByDescending(a => a.AccessTokenExpiryTime).FirstOrDefault();

                if (loginHistory != null)
                {
                    if (loginHistory.AccessToken_Revoke == false)
                    {
                        TimeSpan timeDiff = DateTime.Now.Subtract(loginHistory.AccessTokenExpiryTime);
                        if (timeDiff.TotalMinutes < 20)
                        {
                            IsAnotherUserLogin = true;
                        }
                    }
                }
                return IsAnotherUserLogin;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public long UpdateRefreshToken(string IP, User userModel, string AccessToken, string RefreshToken)
        {
            try
            {
                LoginHistory loginHistory = _userContext.loginHistories.Where(a => a.User.UserName == userModel.UserName && a.MacAdress == IP)
                    .OrderByDescending(a => a.AccessTokenExpiryTime).FirstOrDefault();

                if (loginHistory == null)
                    return 0;
                else
                {
                    loginHistory.Access_Token = AccessToken;
                    loginHistory.RefreshToken = RefreshToken;
                    loginHistory.IsActive = true;
                    loginHistory.ModifyLogin = DateTime.Now;
                    loginHistory.RefreshToken_Revoke = false;
                    loginHistory.AccessToken_Revoke = false;
                    loginHistory.AccessTokenExpiryTime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                    loginHistory.RefreshTokenExpiryTime = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

                    _userContext.SaveChanges();
                    return loginHistory.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public long RevokeRefreshToken(string IP, User user, string AccessToken, string RefreshToken)
        {
            try
            {
                LoginHistory loginHistory = _userContext.loginHistories.Where(a => a.User.UserName == user.UserName && a.MacAdress == IP)
                    .OrderByDescending(a => a.AccessTokenExpiryTime).FirstOrDefault();
                if (loginHistory == null)
                    return 0;
                else
                {
                    loginHistory.IsActive = false;
                    loginHistory.RefreshToken_Revoke = true;
                    loginHistory.AccessToken_Revoke = true;

                    _userContext.SaveChanges();

                    return loginHistory.Id;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
