using System;
using MediatR;
using SharedKernel.Responses;

namespace Identity.API.Application.Commands.Models
{
    public sealed class CancelUserCommandModel : IRequest<CommandResponse>
    {
        public Guid UserId { get; private set; }

        public CancelUserCommandModel(Guid userId)
        {
            UserId = userId;
        }
    }
}