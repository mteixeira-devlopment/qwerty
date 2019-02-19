using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels
{
    public class SignInUser
    {
        [Required]
        public string UserIdentity { get; set; }

        [Required]
        public string Password { get; set; }

        public SignInUser(string userIdentity, string password)
        {
            UserIdentity = userIdentity;
            Password = password;
        }
    }
}