using System.Threading.Tasks;
using Bus.Events;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Hubs;

namespace Notification.API.Bus.Handlers
{
    //public class UserInvalidatedHandler : IHandleMessages<UserInvalidatedEvent>
    //{
    //    private readonly IHubContext<SignUpHub, ITypedHubClient> _hubContext;

    //    public UserInvalidatedHandler(
    //        IHubContext<SignUpHub, ITypedHubClient> hubContext)
    //    {
    //        _hubContext = hubContext;
    //    }

    //    public async Task Handle(UserInvalidatedEvent message, IMessageHandlerContext context)
    //    {
    //        await _hubContext.Clients.All.Notify();
    //    }
    //}
}