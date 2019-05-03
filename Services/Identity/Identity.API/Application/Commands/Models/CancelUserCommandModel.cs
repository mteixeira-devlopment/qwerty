using System;
using MediatR;

namespace Identity.API.Application.Commands.Models
{
    public sealed class CancelUserCommandModel : IRequest<bool>
    {
        public Guid UserId { get; private set; }

        public CancelUserCommandModel(Guid userId)
        {
            UserId = userId;
        }
    }
}