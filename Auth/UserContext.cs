using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Auth_ThirdExample.Auth
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginHistory> loginHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                RoleName = "Manager",
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "johndoe",
                Password = "def@123",
                RoleId = 1
            });
        }
    }
}
