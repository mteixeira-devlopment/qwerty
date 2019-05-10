using System;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class AccountCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }
        public string AccountNumber { get; private set; }

        public AccountCreatedIntegrationEvent(Guid userId, string accountNumber)
        {
            UserId = userId;
            AccountNumber = accountNumber;
        }
    }
}