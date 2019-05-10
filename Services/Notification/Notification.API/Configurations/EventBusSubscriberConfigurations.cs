using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Notification.API.Application.IntegrationEvents.EventHandlers;
using Notification.API.Application.IntegrationEvents.Events;

namespace Notification.API.Configurations
{
    internal static class EventBusSubscriberConfigurations
    {
        public static void ConfigureEventBusSubscribers(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<AccountCreatedIntegrationEvent, AccountCreatedIntegrationEventHandler>();
            eventBus.Subscribe<AccountInvalidatedIntegrationEvent, AccountInvalidatedIntegrationEventHandler>();
        }
    }
}