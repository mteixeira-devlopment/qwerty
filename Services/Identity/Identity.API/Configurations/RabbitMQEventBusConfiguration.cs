using EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Identity.API.Configurations
{
    internal static class RabbitMQEventBusConfiguration
    {
        public static void ConfigureRabbitMQEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersisterConnection>(persister =>
            {
                var logger = persister.GetRequiredService<ILogger<DefaultRabbitMQPersisterConnection>>();

                var eventBusConnectionStrings = GetEventBusConfigurationStrings(configuration);

                var factory = new ConnectionFactory
                {
                    HostName = eventBusConnectionStrings.connection,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(eventBusConnectionStrings.username))
                    factory.UserName = eventBusConnectionStrings.username;

                if (!string.IsNullOrEmpty(eventBusConnectionStrings.password))
                    factory.Password = eventBusConnectionStrings.password;

                var retryCount = 5;
                if (!string.IsNullOrEmpty(eventBusConnectionStrings.retryCount))
                    retryCount = int.Parse(eventBusConnectionStrings.retryCount);

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