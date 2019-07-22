using System;
using EventBus.Events;

namespace Notification.API.Application.IntegrationEvents.Events
{
    public class AccountCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; }
    }
}