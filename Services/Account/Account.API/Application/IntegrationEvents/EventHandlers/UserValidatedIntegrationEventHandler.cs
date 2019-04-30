using System.Threading.Tasks;
using Account.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;

namespace Account.API.Application.IntegrationEvents.EventHandlers
{
    public class UserValidatedIntegrationEventHandler : IIntegrationEventHandler<UserValidatedIntegrationEvent>
    {
        public async Task Handle(UserValidatedIntegrationEvent @event)
        {
            return;
        }
    }
}
