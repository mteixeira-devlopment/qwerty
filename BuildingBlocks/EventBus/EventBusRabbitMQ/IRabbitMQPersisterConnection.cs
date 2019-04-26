using System;
using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public interface IRabbitMQPersisterConnection : IDisposable
    {
        bool IsConnected { get; }
        
        IModel CreateModel();
        bool TryConnect();
    }
}
