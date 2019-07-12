using System;
using System.Collections.Generic;
using System.Linq;
using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionManager
    {
        /// <summary>
        /// Dicionário de manipuladores de assinaturas.
        /// </summary>
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        /// <summary>
        /// Tipos de eventos.
        /// </summary>
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// Assinala se o dicionário de manipuladores de assinaturas está vazia.
        /// </summary>
        public bool IsEmpty => !_handlers.Keys.Any();
        /// <summary>
        /// Limpa o dicionário de manipuladores de assinaturas.
        /// </summary> 
        public void Clear() => _handlers.Clear();

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        /// <summary>
        /// Adicionar uma assinatura dinâmica.
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventname"></param>
        public void AddDynamicSubscription<TH>(string eventname) where TH : IDynamicIntegrationEventHandler
            => DoAddSubscription(typeof(TH), eventname, true);

        /// <summary>
        /// Adiciona uma assinatura tipada, o nome do evento é o nome do tipo do serviço de intergração herdade de IntegrationService. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, false);

            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));
        }

        /// <summary>
        /// Verifica a existência de um assinante para o evento, em caso de não haver, um novo item no dicionário é criado.
        /// Em seguida é verificado a existência de um assinante com o mesmo tipo de manipulador neste mesmo serviço, se sim uma exceção é gerada.
        /// Em caso de fluxo bem sucedido, é adicionado na lista de assinantes no item do dicionário de manipuladores um novo SubscriptionInfo.
        /// </summary>
        /// <param name="handlerType"></param>
        /// <param name="eventName"></param>
        /// <param name="isDynamic"></param>
        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
                _handlers.Add(eventName, new List<SubscriptionInfo>());

            if (_handlers[eventName].Any(h => h.HandlerType == handlerType))
                throw new ArgumentException($"Handler type {handlerType.Name} " +
                                            $"already registered for '{eventName}'", nameof(handlerType));

            _handlers[eventName].Add(
                isDynamic 
                    ? SubscriptionInfo.Dynamic(handlerType) 
                    : SubscriptionInfo.Typed(handlerType));
        }

        /// <summary>
        /// Remove assinaturas dinâmicas do evento informado como parâmetro.
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// Remove assinaturas dinâmicas baseado no tipo do evento informado como parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();

            DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// Remove a assinatura informado da lista no manipulador de assinaturas.
        /// Se não houver outro item na lista deste manipulador, este evento é removido do dicionário de manipuladores.
        /// Em seguida, o tipo do evento é retirado da lista e tipos de eventos. Um evento é disparado quando um evento é removido.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="subscriptionsToRemove"></param>
        private void DoRemoveHandler(string eventName, SubscriptionInfo subscriptionsToRemove)
        {
            if (subscriptionsToRemove == null) return;

            _handlers[eventName].Remove(subscriptionsToRemove);

            if (_handlers[eventName].Any()) return;

            _handlers.Remove(eventName);

            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (eventType != null)
                _eventTypes.Remove(eventType);
                
            RaiseOnEventRemoved(eventName);
        }

        /// <summary>
        /// Evento a ser disparado quando um evento é removido do dicionário de manipuladores.
        /// </summary>
        /// <param name="eventName"></param>
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        /// <summary>
        /// Encontra e retorna a assinatura do evento informado como parâmetro.
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
            => DoFindSubscriptionToRemove(eventName, typeof(TH));

        /// <summary>
        /// Encontra e retorna a assinatura do evento baseado no tipo informado como parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <returns></returns>
        private SubscriptionInfo FindSubscriptionToRemove<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventname = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventname, typeof(TH));
        }
       
        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            return HasSubscriptionsForEvent(eventName) 
                ? null 
                : _handlers[eventName].SingleOrDefault(h => h.HandlerType == handlerType);
        }

        /// <summary>
        /// Verifica se há assinantes para o evento baseado no tipo do evento informado como parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        /// <summary>
        /// Verifica se há assinantes para o evento informado como parâmetro.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool HasSubscriptionsForEvent(string eventName)
            => _handlers.ContainsKey(eventName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public Type GetEventTypeByName(string eventName)
            => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        /// <summary>
        /// Obtem todos os manipuladores registrados do evento baseado pelo tipo informado como parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        /// <summary>
        /// Obtem todos os manipuladores registrados do evento informado como parâmetro.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
            => _handlers[eventName];

        /// <summary>
        /// Obtem o nome do tipo passado como parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetEventKey<T>()
            => typeof(T).Name;

        /// <summary>
        /// Classe interna de informações de assinaturas.
        /// </summary>
        public class SubscriptionInfo
        {
            public bool IsDynamic { get; }
            public Type HandlerType { get; }

            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            public static SubscriptionInfo Dynamic(Type handlerType) => new SubscriptionInfo(true, handlerType);
            public static SubscriptionInfo Typed(Type handlerType) => new SubscriptionInfo(false, handlerType);
        }
    }
}