using System.Threading.Tasks;
using Account.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;

namespace Account.API.Application.IntegrationEvents.EventHandlers
{
    public class UserValidatedIntegrationEventHandler : IIntegrationEventHandler<UserValidatedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;

        public UserValidatedIntegrationEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task Handle(UserValidatedIntegrationEvent @event)
        {
            var accountInvalidated = new AccountInvalidatedIntegrationEvent(@event.UserId, new [] { $"Este documento {@event.Document} já existe vinculado a outra conta." });
            Task.Run(() => _eventBus.Publish(accountInvalidated));
        }
    }
}
