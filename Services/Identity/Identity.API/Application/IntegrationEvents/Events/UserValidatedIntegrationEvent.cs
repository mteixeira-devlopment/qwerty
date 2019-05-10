using System;
using EventBus.Events;

namespace Identity.API.Application.IntegrationEvents.Events
{
    public class UserValidatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }

        public string FullName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Document { get; private set; }

        public UserValidatedIntegrationEvent(Guid userId, string fullName, DateTime birthDate, string document)
        {
            UserId = userId;

            FullName = fullName;
            BirthDate = birthDate;
            Document = document;
        }
    }
}
