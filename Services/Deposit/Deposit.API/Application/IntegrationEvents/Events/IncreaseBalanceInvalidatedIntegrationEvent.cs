using System;
using System.Collections.Generic;
using EventBus.Events;

namespace Deposit.API.Application.IntegrationEvents.Events
{
    public class IncreaseBalanceInvalidatedIntegrationEvent : IntegrationEvent
    {
        public Guid DepositId { get; set; }
        public ICollection<string> ErrorMessages { get; set; }
    }
}