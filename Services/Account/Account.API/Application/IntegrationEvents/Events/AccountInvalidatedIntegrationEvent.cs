using System;
using System.Collections.Generic;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class AccountInvalidatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }
        public ICollection<string> ErrorMessages { get; private set; }

        public AccountInvalidatedIntegrationEvent(Guid userId, ICollection<string> errorMessages)
        {
            UserId = userId;
            ErrorMessages = errorMessages;
        }
    }
}