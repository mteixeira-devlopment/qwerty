using EventBus.Events;

namespace EventBusRabbitMQ.Events
{
    public class ErrorIntegrationEvent : IntegrationEvent
    {
        public string EventName { get; private set; }
        public string QueueName { get; private set; }
        public string ErrorMessage { get; private set; }

        public ErrorIntegrationEvent(string eventName, string queueName, string errorMessage)
        {
            EventName = eventName;
            QueueName = queueName;
            ErrorMessage = errorMessage;
        }
    }
}
