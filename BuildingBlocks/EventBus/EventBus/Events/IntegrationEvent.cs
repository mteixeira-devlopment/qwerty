using System;

namespace EventBus.Events
{
    public class IntegrationEvent
    {
        public Guid EventId { get; }
        public DateTime CreationDate { get; }

        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
