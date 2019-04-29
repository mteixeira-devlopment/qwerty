using System;
using System.Text;
using Autofac;
using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "qwerty_event_bus";

        private readonly IRabbitMQPersisterConnection _persister;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionManager _subsscriptionManager;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFACE_SCOPE_NAME = "qwerty_event_bus";
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

            _consumerChannel = new CreateConsumerChannel();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Publish(IntegrationEvent @event)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void SubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDynamic<T>(string eventName) where T : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persister.IsConnected) _persister.TryConnect();

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

        }
    }
}