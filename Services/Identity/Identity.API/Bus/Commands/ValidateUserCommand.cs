using System;

// ReSharper disable once CheckNamespace
namespace Identity.API.Bus.Commands
{
    public sealed class ValidateUserCommand
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }
    }
}