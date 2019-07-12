using System.Threading.Tasks;
using Deposit.API.Application.IntegrationEvents.Events;
using Deposit.API.Domain.Commands.CancelDepositCharge;
using EventBus.Abstractions;
using MediatR;

namespace Deposit.API.Application.IntegrationEvents.EventHandlers
{
    public class IncreaseBalanceInvalidatedIntegrationEventHandler : IIntegrationEventHandler<IncreaseBalanceInvalidatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public IncreaseBalanceInvalidatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(IncreaseBalanceInvalidatedIntegrationEvent @event)
        {
            var cancelDepositChargeCommandModel = new CancelDepositChargeCommandModel(@event.DepositId, @event.ErrorMessages);
            await _mediator.Send(cancelDepositChargeCommandModel);
        }

        public void Handle2()
        {
            throw new System.NotImplementedException();
        }
    }
}