using System;
using System.Security.Principal;
using Identity.API.Entities;

namespace Identity.API.Models
{
    public sealed class ApplicationUser : Entity<ApplicationUser>
    {
        public string Username { get; private set; }

        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }

        private ApplicationUser()
        {
            
        }

        public ApplicationUser(string username)
        {
            Username = username;
        }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void SetSecurityStamp(string securityStamp)
        {
            SecurityStamp = securityStamp;
        }
    }
}
