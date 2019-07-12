using Deposit.API.Application.IntegrationEvents.EventHandlers;
using Deposit.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Deposit.API.Configurations
{
    public static class EventBusSubscriberConfigurations
    {
        public static void ConfigureEventBusSubscribers(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<IncreaseBalanceInvalidatedIntegrationEvent, IncreaseBalanceInvalidatedIntegrationEventHandler>();
        }
    }
}
