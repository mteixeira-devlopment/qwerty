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

        public async Task Handle(DepositCreatedIntegrationEvent @event)
        {
            var inscreaseBalanceCommandModel = new IncreaseBalanceCommandModel(@event.DepositId, @event.AccountId, @event.Value);
            await _mediator.Send(inscreaseBalanceCommandModel);
        }

        public void Handle2()
        {
            throw new System.NotImplementedException();
        }
    }
}