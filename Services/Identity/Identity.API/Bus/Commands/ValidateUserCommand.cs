using System;

namespace Bus.Commands
{
    public sealed class ValidateUserCommand
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}