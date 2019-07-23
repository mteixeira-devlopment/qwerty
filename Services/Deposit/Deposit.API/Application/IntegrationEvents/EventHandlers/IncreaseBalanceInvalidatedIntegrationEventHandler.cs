using System.Threading.Tasks;
using Deposit.API.Application.IntegrationEvents.Events;
using Deposit.API.Domain.Commands.CancelDepositCharge;
using EventBus.Abstractions;
using MediatR;
using ServiceSeed.Commands;

namespace Deposit.API.Application.IntegrationEvents.EventHandlers
{
    public class IncreaseBalanceInvalidatedIntegrationEventHandler : IIntegrationEventHandler<IncreaseBalanceInvalidatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public IncreaseBalanceInvalidatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IncreaseBalanceInvalidatedIntegrationEvent @event)
        {
            var cancelDepositChargeCommandModel = new CancelDepositChargeCommandModel(@event.DepositId, @event.ErrorMessages);
            var cancelDepositChargeCommandExecution = await _mediator.Send(cancelDepositChargeCommandModel);

            return cancelDepositChargeCommandExecution.ExecutionResult != (int) CommandExecutionResponseTypes.ExecutionFailure;
        }
    }
}