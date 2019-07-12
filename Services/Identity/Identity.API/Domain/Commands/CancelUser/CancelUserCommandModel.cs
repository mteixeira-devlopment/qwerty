using System;
using MediatR;
using ServiceSeed.Responses;

namespace Identity.API.Domain.Commands.CancelUser
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