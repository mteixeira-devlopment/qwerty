using System.Threading.Tasks;
using Bus.Events;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Hubs;
using NServiceBus;

namespace Notification.API.Bus.Handlers
{
    public class UserInvalidatedHandler : IHandleMessages<UserInvalidatedEvent>
    {
        private readonly IHubContext<SignUpHub, ITypedHubClient> _hubContext;
        //private readonly SignUpHub _signUpHub;

        public UserInvalidatedHandler(
            IHubContext<SignUpHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
            //_signUpHub = signUpHub;
        }

        public async Task Handle(UserInvalidatedEvent message, IMessageHandlerContext context)
        {
            await _hubContext.Clients.All.Notify();
        }
    }
}