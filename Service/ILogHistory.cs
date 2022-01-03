using JWT_Auth_ThirdExample.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Service
{
    public interface ILogHistory
    {
        long UpdateLoginHistory(string IP, User userModel, string AccessToken, string RefreshToken);
        bool UserLoggedInDifferentIP(string IP, User userModel);
        bool DifferentIPLoggedIn(string IP, User userModel, string AccessToken, string RefreshToken);
        long UpdateRefreshToken(string IP, User userModel, string AccessToken, string RefreshToken);
        long RevokeRefreshToken(string IP, User user, string AccessToken, string RefreshToken);
    }
}
