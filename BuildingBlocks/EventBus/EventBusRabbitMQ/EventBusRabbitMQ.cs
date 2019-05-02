using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using EventBus.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "qwerty_event_bus";

        private readonly IRabbitMQPersisterConnection _persister;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionManager _subscriptionManager;
        private readonly ILifetimeScope _autofac;

        private readonly string AUTOFAC_SCOPE_NAME = "qwerty_event_bus";
        private readonly int _retryCount;

        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(
            IRabbitMQPersisterConnection persister, 
            ILogger<EventBusRabbitMQ> logger,
            IEventBusSubscriptionManager subscriptionManager,
            ILifetimeScope autofac,             
            string queueName = null, 
            int retryCount = 5)
        {
            _persister = persister ?? throw new ArgumentNullException(nameof(persister));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionManager = subscriptionManager ?? new InMemoryEventBusSubscriptionsManager();
            _autofac = autofac;

            _queueName = queueName;
            _retryCount = retryCount;

            _consumerChannel = CreateConsumerChannel();

            _subscriptionManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persister.IsConnected) _persister.TryConnect();

            using (var channel = _persister.CreateModel())
            {
                channel.QueueUnbind(_queueName, BROKER_NAME, eventName);

                if (!_subscriptionManager.IsEmpty) return;

                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persister.IsConnected)
                _persister.TryConnect();

            var channel = _persister.CreateModel();

            channel.ExchangeDeclare(BROKER_NAME, "direct");
            channel.QueueDeclare(_queueName, true, false, false, null);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();

                StartBasicConsume();
            };

            return channel;
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_persister.IsConnected)
                _persister.TryConnect();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                    (ex, time) 
                        => _logger.LogWarning(
                            ex, 
                            $"Could not publish event: {@event.Id} after {time.TotalSeconds:N1}s", 
                            ex.Message));

            using (var channel = _persister.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.ExchangeDeclare(BROKER_NAME, "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;

                    channel.BasicPublish(BROKER_NAME, eventName, true, properties, body);
                });
            }
        }

        public void SubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation($"Subscription to event {eventName} with {typeof(T).GetGenericTypeDefinition()}");

            DoInternalSubscription(eventName);

            _subscriptionManager.AddDynamicSubscription<T>(eventName);
            StartBasicConsume();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subscriptionManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _logger.LogInformation($"Subscribing to event {eventName} with {typeof(TH).GetGenericTypeName()}");

            _subscriptionManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subscriptionManager.HasSubscriptionsForEvent(eventName);
            if (containsKey) return;

            if (!_persister.IsConnected)
                _persister.TryConnect();

            using (var channel = _persister.CreateModel())
                channel.QueueBind(_queueName, BROKER_NAME, eventName);
        }

        public void UnsubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler
        {
            _subscriptionManager.RemoveDynamicSubscription<T>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subscriptionManager.GetEventKey<T>();

            _logger.LogInformation($"Unsubscriibing from event {eventName}");
            _subscriptionManager.RemoveSubscription<T, TH>();
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ConsumerReceived;

                _consumerChannel.BasicConsume(_queueName, false, consumer);
            }
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                    throw new InvalidOperationException($"Fake exception requested: '{message}'");

                await ProcessEvent(eventName, message);
            } catch (Exception exception)
            {
                _logger.LogWarning(exception, $"***** ERROR Processing message {message}");
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler 
                                = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;

                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var eventType = _subscriptionManager.GetEventTypeByName(eventName);

                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await (Task) concreteType.GetMethod("Handle")
                                .Invoke(handler, new object[] {integrationEvent});
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();

            _subscriptionManager.Clear();
        }
    }
}