using System;
using EventBus.Events;

namespace Deposit.API.Application.IntegrationEvents.Events
{
    public class DepositCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid DepositId { get; private set; }

        public Guid AccountId { get; private set; }
        public decimal Value { get; private set; }

        public DepositCreatedIntegrationEvent(Guid depositId, Guid accountId, decimal value)
        {
            DepositId = depositId;

            AccountId = accountId;
            Value = value;
        }
    }
}
