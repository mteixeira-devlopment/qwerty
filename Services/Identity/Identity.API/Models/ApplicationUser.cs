using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public sealed class ApplicationUser : IdentityUser
    {
        protected ApplicationUser()
        {
            
        }

        public ApplicationUser(string username)
        {
            UserName = username;
        }
    }
}
