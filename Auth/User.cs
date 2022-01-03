using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Auth
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public class LoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserID { get; set; }
        public string MacAdress { get; set; }
        public string Access_Token { get; set; }
        public string RefreshToken { get; set; }
        public bool RefreshToken_Revoke { get; set; }
        public bool AccessToken_Revoke { get; set; }
        public bool IsActive { get; set; }
        public DateTime LoginDate { get; set; }

        public DateTime ModifyLogin { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public User User { get; set; }
    }

    public class Role
    {
        public long Id { get; set; }

        public string RoleName { get; set; }
    }
}
