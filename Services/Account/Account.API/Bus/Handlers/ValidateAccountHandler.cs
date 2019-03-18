using System.Threading.Tasks;
using Bus.Commands;
using NServiceBus;

namespace Account.API.Bus.Handlers
{
    public class ValidateAccountHandler : IHandleMessages<ValidateAccountCommand>
    {
        public Task Handle(ValidateAccountCommand message, IMessageHandlerContext context)
        {
            // Do something with the message here
            return Task.CompletedTask;
        }
    }
}