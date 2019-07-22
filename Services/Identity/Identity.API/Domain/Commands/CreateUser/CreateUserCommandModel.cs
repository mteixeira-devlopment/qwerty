using System;
using MediatR;
using ServiceSeed.Responses;

namespace Identity.API.Domain.Commands.CreateUser
{
    public sealed class CreateUserCommandModel : IRequest<CommandResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }

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
