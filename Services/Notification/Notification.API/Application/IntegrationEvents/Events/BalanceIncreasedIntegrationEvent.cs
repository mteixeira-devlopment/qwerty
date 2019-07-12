using System;
using EventBus.Events;

namespace Notification.API.Application.IntegrationEvents.Events
{
    public class BalanceIncreasedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public decimal Value { get; set; }
    }
}