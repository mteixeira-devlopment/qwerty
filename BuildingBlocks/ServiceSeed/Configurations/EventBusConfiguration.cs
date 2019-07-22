using EventBus;
using EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using EvBusRabbitMQ = EventBusRabbitMQ.EventBusRabbitMQ;

namespace ServiceSeed.Configurations
{
    public static class EventBusConfiguration
    {
        /// <summary>
        /// Injecão do event bus do RabbitMq.
        /// Incluindo o persister do RabbitMq, logger e o gerenciar de eventos e manipuladores em memória.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var clientName = configuration["SubscriptionClientName"];

            var retryCount = 5;
            if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                retryCount = int.Parse(configuration["EventBusRetryCount"]);

            services.AddSingleton<IEventBus, EvBusRabbitMQ>(
                serviceProvider => new EvBusRabbitMQ(serviceProvider, clientName, retryCount));

            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}