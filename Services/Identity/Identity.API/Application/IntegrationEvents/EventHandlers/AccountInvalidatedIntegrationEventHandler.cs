using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.API.Domain.Commands.CancelUser;
using MediatR;

namespace Identity.API.Application.IntegrationEvents.EventHandlers
{
    public class AccountInvalidatedIntegrationEventHandler : IIntegrationEventHandler<AccountInvalidatedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public AccountInvalidatedIntegrationEventHandler(
            IMediator mediator, 
            IEventBus eventBus)
        {
            _mediator = mediator;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(AccountInvalidatedIntegrationEvent @event)
        {
            var cancelUserCommandModel = new CancelUserCommandModel(@event.UserId);
            var cancelUserCommandExecution = await _mediator.Send(cancelUserCommandModel);

            return cancelUserCommandExecution.Success;
        }
    }
}