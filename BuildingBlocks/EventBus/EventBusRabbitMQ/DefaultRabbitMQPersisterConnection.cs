using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ
{
    public class DefaultRabbitMQPersisterConnection : IRabbitMQPersisterConnection
    {
        /// <summary>
        /// Connection factory do rabbit client.
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        private readonly ILogger<DefaultRabbitMQPersisterConnection> _logger;

        /// <summary>
        /// Contador de tentativas.
        /// </summary>
        private readonly int _retryCount;

        /// <summary>
        /// Conexão AMQP.
        /// </summary>
        private IConnection _connection;
        private bool _disposed;

        /// <summary>
        /// Identificador de lock para efeturar tentativa de conexaõ.
        /// </summary>
        private static readonly object SyncRoot = new object();

        public DefaultRabbitMQPersisterConnection(
            IConnectionFactory connectionFactory, 
            ILogger<DefaultRabbitMQPersisterConnection> logger, 
            int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        /// <summary>
        /// Cria e retorna um canal, sessão e modelo para a conexão.
        /// </summary>
        /// <returns></returns>
        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nenhuma conexão com RabbitMq está disponível para completar esta ação.");

            return _connection.CreateModel();
        }

        /// <summary>
        /// Executa tentativa de conexão com o RabbitMq considerando uma política de reconexão.
        /// </summary>
        /// <returns></returns>
        public bool TryConnect()
        {
            _logger.LogInformation("Tentativa de conexão com RabbitMq iniciando...");

            lock (SyncRoot)
            {
                var policy = Policy
                    .Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(
                        _retryCount,
                        retryAttempt => 
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                            (ex, time) => _logger.LogWarning($"Tentativa de reconexão. Motivo: {ex.ToString()}"));

                policy.Execute(() => _connection = _connectionFactory.CreateConnection());

                if (IsConnected)
                {
                    HandlerConnectedPersister();
                    return true;
                }

                _logger.LogCritical(@"FATAL ERROR: Não foi possível abrir ou criar 
                                    uma conexão com RabbitMq!");
                return false;
            }
        }

        /// <summary>
        /// Inclui os eventos para controle de fluxo para a conexão obtida.
        /// </summary>
        private void HandlerConnectedPersister()
        {
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;

            _logger.LogInformation(
                $"Conexão adquiria com RabbitMq {_connection.Endpoint.HostName}.");
        }

        public void Dispose()
        {
            if (!_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException exception)
            {
                _logger.LogCritical($"Erro ao encerrar conexão: {exception.Message}");
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A conexão com o RabbiMq foi interrompida. Tente conectar novamente...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A conexão com o RabbitMq lançou uma excessão. Tente conectar novamente...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A conexão com o RabbiMq foi encerrada. Tente conectar novamente...");

            TryConnect();
        }
    }
}