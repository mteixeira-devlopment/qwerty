using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels
{
    public class SignInUser
    {
        [Required]
        public string UserIdentity { get; }

        [Required]
        public string Password { get; }

        public SignInUser(string userIdentity, string password)
        {
            UserIdentity = userIdentity;
            Password = password;
        }
    }
}