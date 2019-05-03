using System;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class AccountInvalidatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }
        public string[] ErrorMessage { get; private set; }

        public AccountInvalidatedIntegrationEvent(Guid userId, string[] errorMessage)
        {
            UserId = userId;
            ErrorMessage = errorMessage;
        }
    }
}