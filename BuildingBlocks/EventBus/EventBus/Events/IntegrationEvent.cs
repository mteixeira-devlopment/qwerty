using System;

namespace EventBus.Events
{
    /// <summary>
    /// Classe base para os serviços de integração.
    /// </summary>
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
