using System;
using System.Security.Principal;
using Identity.API.Entities;

namespace Identity.API.Models
{
    public sealed class ApplicationUser : Entity<ApplicationUser>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        protected ApplicationUser()
        {
            
        }

        public ApplicationUser(string username)
        {
            Username = username;
        }
    }
}
