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
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMQPersisterConnection> _logger;
        private readonly int _retryCount;

        private IConnection _connection;
        private bool _disposed;

        private readonly object sync_root = new object();

        public DefaultRabbitMQPersisterConnection(
            IConnectionFactory connectionFactory, ILogger<DefaultRabbitMQPersisterConnection> logger, int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("No RabbitMQ connection are available to perform this action");

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ client is trying to connect.");

            lock (sync_root)
            {
                var policy = Policy
                    .Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(
                        _retryCount,
                        retryAttempt => 
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                        (ex, time) => _logger.LogWarning(ex.ToString()));

                policy.Execute(() => _connection = _connectionFactory.CreateConnection());

                if (IsConnected)
                {
                    HandlerConnectedPersister();
                    return true;
                }

                _logger.LogCritical($"FATAL ERROR: RabbitMQ connections could not be created and opened.");
                return false;
            }
        }

        private void HandlerConnectedPersister()
        {
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;

            _logger.LogInformation(
                $"RabbitMQ persistent connection acquired a connection " +
                $"{_connection.Endpoint.HostName} and is subscribed to failure events.");
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
                _logger.LogCritical(exception.ToString());
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}