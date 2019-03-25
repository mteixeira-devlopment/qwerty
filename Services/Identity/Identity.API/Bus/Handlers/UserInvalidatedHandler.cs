using System.Threading.Tasks;
using Bus.Events;
using NServiceBus;

namespace Identity.API.Bus.Handlers
{
    public class UserInvalidatedHandler : IHandleMessages<UserInvalidatedEvent>
    {
        public async Task Handle(UserInvalidatedEvent message, IMessageHandlerContext context)
        {
            return;
        }
    }
}