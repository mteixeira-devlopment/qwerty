using System;
using MediatR;
using ServiceSeed.Responses;

namespace Account.API.Domain.Commands.CreateAccount
{
    public class CreateAccountCommandModel : IRequest<CommandResponse>
    {
        public Guid UserId { get; private set; }

        public string FullName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Document { get; private set; }

        public CreateAccountCommandModel(Guid userId, string fullName, DateTime birthDate, string document)
        {
            UserId = userId;

            FullName = fullName;
            BirthDate = birthDate;
            Document = document;
        }
    }
}
