using System;
using System.Collections.Generic;
using EventBus.Events;

namespace Notification.API.Application.IntegrationEvents.Events
{
    public class DepositCanceledIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public ICollection<string> CancelationReasons { get; set; }
       
    }
}