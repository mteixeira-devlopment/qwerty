using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Events;

namespace Identity.API.Application.IntegrationEvents.Events
{
    public class UserValidatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }

        public UserValidatedIntegrationEvent(Guid userId, string fullName, DateTime birthDate, string document)
        {
            UserId = userId;

            FullName = fullName;
            BirthDate = birthDate;
            Document = document;
        }
    }
}
