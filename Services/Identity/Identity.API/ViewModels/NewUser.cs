using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels
{
    public class NewUser
    {
        [Required]
        [MinLength(3)]
        public string Username { get; }

        [Required]
        [MinLength(6)]
        [MaxLength(21)]
        public string Password { get; }

        public NewUser(string password, string username)
        {
            Username = username;
            Password = password;
        }
    }
}