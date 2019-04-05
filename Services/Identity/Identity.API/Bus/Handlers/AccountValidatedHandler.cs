using System.Threading.Tasks;
using Bus.Events;
using NServiceBus;

namespace Identity.API.Bus.Handlers
{
    public class AccountValidatedHandler : IHandleMessages<AccountValidatedEvent>
    {
        public Task Handle(AccountValidatedEvent message, IMessageHandlerContext context)
        {
            // Notify client
            return Task.CompletedTask;
        }
    }
}