using System;
using Account.API.Domain;

// ReSharper disable once CheckNamespace
namespace Bus.Commands
{
    public sealed class ValidateAccountCommand
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }
    }
}
