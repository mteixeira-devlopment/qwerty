﻿using System;
using MediatR;

namespace Identity.API.Domain.Commands
{
    public sealed class CreateUserCommandModel : IRequest<bool>
    {
        public Guid UserId { get; private set; }

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
