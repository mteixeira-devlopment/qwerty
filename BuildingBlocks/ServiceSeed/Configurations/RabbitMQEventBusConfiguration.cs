using EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ServiceSeed.Configurations
{
    public static class RabbitMQEventBusConfiguration
    {
        public static void ConfigureRabbitMQEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersisterConnection>(persister =>
            {
                var logger = persister.GetRequiredService<ILogger<DefaultRabbitMQPersisterConnection>>();

                var (connection, username, password, s) = GetEventBusConfigurationStrings(configuration);

                var factory = new ConnectionFactory
                {
                    HostName = connection,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(username))
                    factory.UserName = username;

                if (!string.IsNullOrEmpty(password))
                    factory.Password = password;

                var retryCount = 5;
                if (!string.IsNullOrEmpty(s))
                    retryCount = int.Parse(s);

                return new DefaultRabbitMQPersisterConnection(factory, logger, retryCount);

            });
        }

        private static (string connection, string username, string password, string retryCount)
            GetEventBusConfigurationStrings(IConfiguration configuration)
            => (configuration["EventBusConnection"], 
                configuration["EventBusUserName"],
                configuration["EventBusPassword"], 
                configuration["EventBusRetryCount"]);
    }
}