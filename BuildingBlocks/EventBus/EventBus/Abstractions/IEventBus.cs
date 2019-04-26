using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void SubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler;

        void UnsubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler;

        void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}