using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        /// <summary>
        /// Publica um evento para a fila do serviço e chave de rota (routing key) deste evento.
        /// </summary>
        /// <param name="event"></param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// Assina dinamicamente um evento com um handler específico.
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Assina um evento de um tipo e handler específico.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Remove a assinatura de um evento dinâmico.
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Remove a assinatura de um evento de tipo específico.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}