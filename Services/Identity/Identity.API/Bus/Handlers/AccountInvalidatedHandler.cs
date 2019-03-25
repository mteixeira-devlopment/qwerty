using System.Threading.Tasks;
using Bus.Events;
using NServiceBus;

namespace Identity.API.Bus.Handlers
{
    public class AccountInvalidatedHandler : IHandleMessages<AccountInvalidatedEvent>
    {
        public async Task Handle(AccountInvalidatedEvent message, IMessageHandlerContext context)
        {
            return;
        }
    }
}