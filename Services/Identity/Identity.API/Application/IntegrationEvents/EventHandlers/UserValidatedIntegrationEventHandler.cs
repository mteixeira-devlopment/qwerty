using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.Events;
using MediatR;

namespace Identity.API.Application.IntegrationEvents.EventHandlers
{
    public class UserValidatedIntegrationEventHandler : IIntegrationEventHandler<UserValidatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public UserValidatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(UserValidatedIntegrationEvent @event)
        {

            return true;
        }
    }
}
