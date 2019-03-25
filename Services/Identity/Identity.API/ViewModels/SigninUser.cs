﻿using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels
{
    public class SignInUser
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public SignInUser(string userIdentity, string password)
        {
            Username = userIdentity;
            Password = password;
        }
    }
}