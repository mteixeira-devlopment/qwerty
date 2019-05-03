using System;

// ReSharper disable once CheckNamespace
namespace Identity.API.Bus.Commands
{
    public sealed class ValidateAccountCommand
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }

        public ValidateAccountCommand(Guid userId, string fullName, DateTime birthDate, string document)
        {
            UserId = userId;
            FullName = fullName;
            BirthDate = birthDate;
            Document = document;
        }
    }
}
