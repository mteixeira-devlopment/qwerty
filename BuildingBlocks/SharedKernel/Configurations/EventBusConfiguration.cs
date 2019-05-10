using Autofac;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using EvBusRabbitMQ = EventBusRabbitMQ.EventBusRabbitMQ;

namespace SharedKernel.Configurations
{
    public static class EventBusConfiguration
    {
        public static void ConfigureEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            services.AddSingleton<IEventBus, EvBusRabbitMQ>(persister =>
            {
                var rabbitMQpersister = persister.GetRequiredService<IRabbitMQPersisterConnection>();
                var lifetimeScope = persister.GetRequiredService<ILifetimeScope>();

                var logger = persister.GetRequiredService<ILogger<EvBusRabbitMQ>>();

                var eventBusSubscriptionManager = persister.GetRequiredService<IEventBusSubscriptionManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);

                return new EvBusRabbitMQ(rabbitMQpersister, logger, eventBusSubscriptionManager, lifetimeScope, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}