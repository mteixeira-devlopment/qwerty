using System;
using System.Collections.Generic;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class IncreaseBalanceInvalidatedIntegrationEvent : IntegrationEvent
    {
        public Guid DepositId { get; private set; }
        public ICollection<string> ErrorMessages { get; private set; }

        public IncreaseBalanceInvalidatedIntegrationEvent(Guid depositId, ICollection<string> errorMessages)
        {
            DepositId = depositId;
            ErrorMessages = errorMessages;
        }
    }
}