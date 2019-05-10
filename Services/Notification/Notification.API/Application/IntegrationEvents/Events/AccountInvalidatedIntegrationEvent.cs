using System;
using EventBus.Events;

namespace Notification.API.Application.IntegrationEvents.Events
{
    public class AccountInvalidatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string[] ErrorMessage { get; set; }
    }
}