using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using EventBus.Extensions;
using EventBusRabbitMQ.Events;
using Microsoft.Extensions.DependencyInjection;
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
        private const string BROKER_NAME = "qwerty_event_bus";
        private const string BROKER_TYPE = "topic";

        private const string ERROR_QUEUE_NAME = "Error";
        private const string AUDIT_QUEUE_NAME = "Audit";
      
        private readonly IRabbitMQPersisterConnection _persister;
        private readonly ILogger<EventBusRabbitMQ> _logger;
       
        private readonly IEventBusSubscriptionManager _subscriptionManager;

        private readonly IServiceProvider _serviceProvider;

        private readonly int _retryCount;

        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(
            IServiceProvider serviceProvider,
            string queueName,
            int retryCount = 5)
        {
            _persister = serviceProvider.GetRequiredService<IRabbitMQPersisterConnection>() 
                         ?? throw new ArgumentNullException(nameof(_persister));

            _logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>() 
                      ?? throw new ArgumentNullException(nameof(_logger));

            _subscriptionManager = serviceProvider.GetRequiredService<IEventBusSubscriptionManager>() 
                                   ?? new InMemoryEventBusSubscriptionsManager();
           
            _serviceProvider = serviceProvider;

            _queueName = queueName;
            _retryCount = retryCount;

            _consumerChannel = CreateConsumerChannel(_queueName);

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

        private IModel CreateConsumerChannel(string queueName)
        {
            if (!_persister.IsConnected)
                _persister.TryConnect();

            var channel = _persister.CreateModel();

            channel.ExchangeDeclare(BROKER_NAME, BROKER_TYPE);
            channel.QueueDeclare(queueName, true, false, false, null);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();

                _consumerChannel = CreateConsumerChannel(queueName);
                StartBasicConsume();
            };

            return channel;
        }
        
        public void Publish(IntegrationEvent @event)
        {
            if (!_persister.IsConnected) _persister.TryConnect();

            var policyBuilder = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>();

            var policy = policyBuilder.WaitAndRetry(
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time)
                        => _logger.LogWarning(
                            ex,
                            $"Não foi possível publicar o evento {@event.EventId} após {time.TotalSeconds:N1}s",
                            ex.Message));

            CreateModelAndPublish(@event, policy);
        }

        private void CreateModelAndPublish(IntegrationEvent @event, RetryPolicy policy)
        {
            using (var channel = _persister.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.ExchangeDeclare(BROKER_NAME, BROKER_TYPE);

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
            _logger.LogInformation($"Assinando o evento {eventName} com {typeof(T).GetGenericTypeDefinition()}");

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

            _logger.LogInformation($"Assinando o evento {eventName} com {typeof(TH).GetGenericTypeName()}");

            _subscriptionManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }
        
        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subscriptionManager.HasSubscriptionsForEvent(eventName);
            if (containsKey) return;

            if (!_persister.IsConnected) _persister.TryConnect();

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

            _logger.LogInformation($"Removendo assinatura de {eventName}");
            _subscriptionManager.RemoveSubscription<T, TH>();
        }
      
        private void StartBasicConsume()
        {
            if (_consumerChannel == null) return;

            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

            consumer.Received += ConsumerReceived;

            _consumerChannel.BasicConsume(_queueName, false, consumer);
        }
     
        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body);
            
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName)){
               
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var policyContext = new Context(eventName);

                    var policyBuilder = Policy
                        .HandleResult<bool>(successfullyExecution => !successfullyExecution)
                        .Or<Exception>();

                    var policy = policyBuilder
                        .WaitAndRetryAsync(
                            _retryCount,
                            retryAttempt => TimeSpan.FromSeconds(1), (executionResult, retryDelay, ctx) => 
                                _logger.LogWarning($@"Falha no processamento da mensagem: {message}, retentativa em {retryDelay} segundos."))
                        .WithPolicyKey(eventName);

                    var policyExecution = await policy.ExecuteAsync(
                        async ctx => await ProcessEvent(eventName, message, subscription).ConfigureAwait(false), policyContext).ConfigureAwait(false);

                    if (!policyExecution)
                    {
                        // todo: adjust error messages to error queue listener
                        var errorIntegrationEvent = new ErrorIntegrationEvent(eventName, _queueName, "Deu ruim oh!");
                        Publish(errorIntegrationEvent);
                    }

                    _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
                }
            }
        }
       
        private async Task<bool> ProcessEvent(
            string eventName, string message, InMemoryEventBusSubscriptionsManager.SubscriptionInfo subscription)
        {
            if (subscription.IsDynamic)
            {
                if (!(_serviceProvider.GetRequiredService(subscription.HandlerType) is IDynamicIntegrationEventHandler handler))
                    throw new Exception($"Erro ao obter o handler do evento {eventName}");

                dynamic eventData = JObject.Parse(message);
                return await handler.Handle(eventData);
            }
            
            var eventType = _subscriptionManager.GetEventTypeByName(eventName);
            
            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);
                
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                var result = (Task<bool>) concreteType
                    .GetMethod("Handle")
                    .Invoke(handler, new[] { integrationEvent });

                return await result;
            }

        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();
            _subscriptionManager.Clear();
        }
    }
}