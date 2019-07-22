using System.Threading.Tasks;
using Account.API.Application.IntegrationEvents.Events;
using Account.API.Domain.Commands.CreateAccount;
using EventBus.Abstractions;
using MediatR;
using ServiceSeed.Commands;

namespace Account.API.Application.IntegrationEvents.EventHandlers
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
            var createAccountCommandModel = new CreateAccountCommandModel(
                @event.UserId, @event.FullName, @event.BirthDate, @event.Document);

            var createAccountCommandExecution = await _mediator.Send(createAccountCommandModel);

            return createAccountCommandExecution.ExecutionResult != (int) CommandExecutionResponseTypes.ExecutionFailure;
        }
    }
}
