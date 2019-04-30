using System;
using EventBus.Events;

namespace Account.API.Application.IntegrationEvents.Events
{
    public class UserValidatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }
    }
}
