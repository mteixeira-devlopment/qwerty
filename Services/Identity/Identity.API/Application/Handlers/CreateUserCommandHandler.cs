using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.API.Domain.Commands;
using MediatR;

namespace Identity.API.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandModel, bool>
    {
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public CreateUserCommandHandler(IMediator mediator, IEventBus eventBus)
        {
            _mediator = mediator;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(CreateUserCommandModel request, CancellationToken cancellationToken)
        {
            var c = new UserValidatedIntegrationEvent(Guid.NewGuid(), "", DateTime.Now, "");

            _eventBus.Publish(c);
            return true;
        }
    }
}
