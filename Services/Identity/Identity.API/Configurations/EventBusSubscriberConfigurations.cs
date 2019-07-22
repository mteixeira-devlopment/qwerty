using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.EventHandlers;
using Identity.API.Application.IntegrationEvents.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Configurations
{
    public static class EventBusSubscriberConfigurations
    {
        public static void ConfigureEventBusSubscribers(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<AccountInvalidatedIntegrationEvent, AccountInvalidatedIntegrationEventHandler>();
        }
    }
}