using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Auth
{
    public static class AuthorizeRoles
    {
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string AdministratorOrUser = Administrator + "," + User;
        public const string ManagerOrAdministrator = Administrator + "," + Manager;
    }
}
