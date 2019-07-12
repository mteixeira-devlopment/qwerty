using Account.API.Application.IntegrationEvents.EventHandlers;
using Account.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Account.API.Configurations
{
    public static class EventBusSubscriberConfigurations
    {
        public static void ConfigureEventBusSubscribers(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserValidatedIntegrationEvent, UserValidatedIntegrationEventHandler>();
            eventBus.Subscribe<DepositCreatedIntegrationEvent, DepositCreatedIntegrationEventHandler>();
        }
    }
}
