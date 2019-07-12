using System;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class DepositCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid DepositId { get; set; }

        public Guid AccountId { get; set; }
        public decimal Value { get; set; }
    }
}