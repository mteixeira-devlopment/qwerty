using System;
using MediatR;
using SharedKernel.Responses;

namespace Account.API.Application.Commands.Models
{
    public class CreateAccountCommandModel : IRequest<CommandResponse>
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }

        public CreateAccountCommandModel(Guid userId, string fullName, DateTime birthDate, string document)
        {
            UserId = userId;

            FullName = fullName;
            BirthDate = birthDate;
            Document = document;
        }
    }
}
