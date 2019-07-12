using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using EventBus.Extensions;
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
        /// <summary>
        /// Nome dado a exchange.
        /// Uma única exchange será utilizada com esta abordagem.
        /// </summary>
        const string BROKER_NAME = "qwerty_event_bus";

        /// <summary>
        /// Owner da conexão com o RabbitMq.
        /// </summary>
        private readonly IRabbitMQPersisterConnection _persister;
        private readonly ILogger<EventBusRabbitMQ> _logger;

        /// <summary>
        /// Gerenciador de eventos e manipuladores em memória.
        /// </summary>
        private readonly IEventBusSubscriptionManager _subscriptionManager;

        /// <summary>
        /// Injetor em tempo de execução para os manipuladores processados.
        /// </summary>
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "qwerty_event_bus";

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

        /// <summary>
        /// Cria o canal consumidor e declara a fila.
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerChannel()
        {
            if (!_persister.IsConnected)
                _persister.TryConnect();

            var channel = _persister.CreateModel();

            channel.ExchangeDeclare(BROKER_NAME, "fanout");
            channel.QueueDeclare(_queueName, true, false, false, null);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();

                StartBasicConsume();
            };

            return channel;
        }

        /// <summary>
        /// Publica um evento recebido por parâmetro.
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IntegrationEvent @event)
        {
            if (!_persister.IsConnected) _persister.TryConnect();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time)
                        => _logger.LogWarning(
                            ex,
                            $"Não foi possível public o evento {@event.EventId} após {time.TotalSeconds:N1}s",
                            ex.Message));

            CreateModel(@event, policy);
        }

        /// <summary>
        /// Obtem um canal, sessão e modelo com base em um evento e uma política de re-tentantivas.
        /// Obtém a exchange do canal e publica a mensagem.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="policy"></param>
        private void CreateModel(IntegrationEvent @event, RetryPolicy policy)
        {
            using (var channel = _persister.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.ExchangeDeclare(BROKER_NAME, "fanout");

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

        /// <summary>
        /// Assina o evento recebido por parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
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

        /// <summary>
        /// Agrega a assinatura interna para o gerenciador de eventos e manipuladores.
        /// Binda o evento à fila do serviço e routing key específica daquele evento.
        /// </summary>
        /// <param name="eventName"></param>
        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subscriptionManager.HasSubscriptionsForEvent(eventName);
            if (containsKey) return;

            if (!_persister.IsConnected) _persister.TryConnect();

            using (var channel = _persister.CreateModel())
                channel.QueueBind(_queueName, BROKER_NAME, eventName);
        }

        /// <summary>
        /// Remove a assinatura do evento recebido por parâmetro.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
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

        /// <summary>
        /// Inicia um consumidor e atribui o evento de consumo.
        /// </summary>
        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ConsumerReceived;

                _consumerChannel.BasicConsume(_queueName, false, consumer);
            }
        }

        /// <summary>
        /// Consome os eventos baseados na chave de rota enviada no eventArgs.
        /// Tenta processar o evento e confirma com o "aceite".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body);

            try
            {
                await ProcessEvent(eventName, message);
                _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);

            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Não foi possível processar a mensagem: {message}");
            }
        }

        /// <summary>
        /// Processa o evento com base no nome recebido por parâmetro.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message)
        {
            // Verifica se há assinantes para o evento.
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                // Obtem os assinantes.
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        // Para assinantes dinâmicos.
                        if (!(_serviceProvider.GetRequiredService(subscription.HandlerType) 
                            is IDynamicIntegrationEventHandler handler))
                                throw new Exception($"Erro ao obter o handler do evento {eventName}");

                        dynamic eventData = JObject.Parse(message);
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        // Para assinantes tipados.
                        var eventType = _subscriptionManager.GetEventTypeByName(eventName);

                        // Obtem o evento a partir da mensagem e do tipo.
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        // Obtem o handler injetado com base no evento.
                        var handler = _serviceProvider.GetRequiredService(subscription.HandlerType);

                        // Obtem o type genérico do handler do evento.
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        // Invoca o método.
                        concreteType.GetMethod("Handle").Invoke(handler, new[] { integrationEvent });
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