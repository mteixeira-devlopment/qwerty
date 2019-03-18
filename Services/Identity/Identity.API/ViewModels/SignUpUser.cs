using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels
{
    public class SignUpUser
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(21)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Document { get; set; }
    }
}