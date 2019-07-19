using System.Linq;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Application.IntegrationEvents.Events;
using Notification.API.Hubs;

namespace Notification.API.Application.IntegrationEvents.EventHandlers
{
    public class AccountInvalidatedIntegrationEventHandler : IIntegrationEventHandler<AccountInvalidatedIntegrationEvent>
    {
        private readonly IHubContext<RequestResponseMessageHub, IRequestResponseMessageHub> _hubContext;

        public AccountInvalidatedIntegrationEventHandler(
            IHubContext<RequestResponseMessageHub, IRequestResponseMessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(AccountInvalidatedIntegrationEvent @event)
        {
            await _hubContext.Clients.All.Notify(@event.ErrorMessage.First());

            return true;
        }
    }
}