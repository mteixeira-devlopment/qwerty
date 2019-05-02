using System;
using System.Collections.Generic;
using System.Linq;
using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionManager
    {
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

    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public bool IsEmpty => !_handlers.Keys.Any();
        public void Clear() => _handlers.Clear();

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public void AddDynamicSubscription<T>(string eventname) where T : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(T), eventname, true);
        }

        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, false);

            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));
        }

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

        public void RemoveDynamicSubscription<T>(string eventName) where T : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<T>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();

            DoRemoveHandler(eventName, handlerToRemove);
        }

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

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
            => DoFindSubscriptionToRemove(eventName, typeof(TH));

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

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName)
            => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName)
            => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
            => _handlers[eventName];

        public string GetEventKey<T>()
            => typeof(T).Name;
    }
}