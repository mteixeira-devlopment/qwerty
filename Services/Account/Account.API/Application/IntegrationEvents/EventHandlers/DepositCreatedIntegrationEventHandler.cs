using System.Threading.Tasks;
using Account.API.Application.IntegrationEvents.Events;
using Account.API.Domain.Commands.IncreaseBalance;
using EventBus.Abstractions;
using MediatR;

namespace Account.API.Application.IntegrationEvents.EventHandlers
{
    public class DepositCreatedIntegrationEventHandler : IIntegrationEventHandler<DepositCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public DepositCreatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(DepositCreatedIntegrationEvent @event)
        {
            var inscreaseBalanceCommandModel = new IncreaseBalanceCommandModel(@event.DepositId, @event.AccountId, @event.Value);
            var inscreaseBalanceCommandExecution = await _mediator.Send(inscreaseBalanceCommandModel);

            return inscreaseBalanceCommandExecution.Success;
        }
    }
}