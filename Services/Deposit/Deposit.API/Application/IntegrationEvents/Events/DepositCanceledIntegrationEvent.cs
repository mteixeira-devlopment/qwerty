using System;
using System.Collections.Generic;
using EventBus.Events;

namespace Deposit.API.Application.IntegrationEvents.Events
{
    public class DepositCanceledIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }
        public ICollection<string> CancelationReasons { get; private set; }

        public DepositCanceledIntegrationEvent(Guid userId, ICollection<string> cancelationReasons)
        {
            UserId = userId;
            CancelationReasons = cancelationReasons;
        }
    }
}