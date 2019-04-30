using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
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
        private readonly IEventBusSubscriptionManager _subsscriptionManager;
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
            _subsscriptionManager = subscriptionManager ?? new InMemoryEventBusSubscriptionsManager();
            _autofac = autofac;

            _queueName = queueName;
            _retryCount = retryCount;

            _consumerChannel = CreateConsumerChannel();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persister.IsConnected)
                _persister.TryConnect();

            var channel = _persister.CreateModel();

            channel.ExchangeDeclare(_queueName, "direct");
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

            _subsscriptionManager.AddDynamicSubscription<T>(eventName);
            StartBasicConsume();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsscriptionManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _logger.LogInformation($"Subscription to event {eventName} with {typeof(TH).GetGenericTypeDefinition()}");

            _subsscriptionManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsscriptionManager.HasSubscriptionsForEvent(eventName);
            if (containsKey) return;

            if (!_persister.IsConnected)
                _persister.TryConnect();

            using (var channel = _persister.CreateModel())
                channel.QueueBind(_queueName, BROKER_NAME, eventName);
        }

        public void UnsubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler
        {
            _subsscriptionManager.RemoveDynamicSubscription<T>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsscriptionManager.GetEventKey<T>();

            _logger.LogInformation($"Unsubscriibing from event {eventName}");
            _subsscriptionManager.RemoveSubscription<T, TH>();
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ConsumerReceived;
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
            if (_subsscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsscriptionManager.GetHandlersForEvent(eventName);
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
                            var eventType = _subsscriptionManager.GetEventTypeByName(eventName);

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

            _subsscriptionManager.Clear();
        }
    }
}