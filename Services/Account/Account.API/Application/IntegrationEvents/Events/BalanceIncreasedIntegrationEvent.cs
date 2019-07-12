using System;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class BalanceIncreasedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }
        public decimal Value { get; private set; }

        public BalanceIncreasedIntegrationEvent(Guid userId, decimal value)
        {
            UserId = userId;
            Value = value;
        }
    }
}