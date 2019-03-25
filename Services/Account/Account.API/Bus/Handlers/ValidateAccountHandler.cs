using System.Threading.Tasks;
using Account.API.Configurations;
using Bus.Commands;
using Bus.Events;
using NServiceBus;

namespace Account.API.Bus.Handlers
{
    public class ValidateAccountHandler : IHandleMessages<ValidateAccountCommand>
    {
        public async Task Handle(ValidateAccountCommand message, IMessageHandlerContext context)
        {
            var accountInvalidatedEvent = new AccountInvalidatedEvent(message.UserId, "Ola!");

            await BusConfiguration
                .BusEndpointInstance
                .Publish(accountInvalidatedEvent)
                .ConfigureAwait(false);
        }
    }
}