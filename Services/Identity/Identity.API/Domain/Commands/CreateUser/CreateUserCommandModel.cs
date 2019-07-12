using System;
using MediatR;
using ServiceSeed.Responses;

namespace Identity.API.Domain.Commands.CreateUser
{
    public sealed class CreateUserCommandModel : IRequest<CommandResponse>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public string FullName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Document { get; private set; }

        public CreateUserCommandModel(
            string username, string password, string fullName, DateTime birthDate, string document)
        {
            Username = username;
            Password = password;
            FullName = fullName;
            BirthDate = birthDate;
            Document = document;
        }
    }
}
