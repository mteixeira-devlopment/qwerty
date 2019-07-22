using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.API.Domain.Commands.CancelUser;
using MediatR;
using ServiceSeed.Commands;

namespace Identity.API.Application.IntegrationEvents.EventHandlers
{
    public class AccountInvalidatedIntegrationEventHandler : IIntegrationEventHandler<AccountInvalidatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public AccountInvalidatedIntegrationEventHandler(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(AccountInvalidatedIntegrationEvent @event)
        {
            var cancelUserCommandModel = new CancelUserCommandModel(@event.UserId);
            var cancelUserCommandExecution = await _mediator.Send(cancelUserCommandModel);

            return cancelUserCommandExecution.ExecutionResult != (int) CommandExecutionResponseTypes.ExecutionFailure;
        }
    }
}