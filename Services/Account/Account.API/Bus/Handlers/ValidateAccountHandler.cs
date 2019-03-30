using System.Threading.Tasks;
using Account.API.Configurations;
using Account.API.Domain;
using Account.API.Domain.Validations;
using Bus.Commands;
using Bus.Events;
using NServiceBus;

using Acc = Account.API.Domain.Account;

namespace Account.API.Bus.Handlers
{
    public class ValidateAccountHandler : IHandleMessages<ValidateAccountCommand>
    {
        public async Task Handle(ValidateAccountCommand message, IMessageHandlerContext context)
        {
            var document = new Document(message.Document);
            var customer = new Customer(message.FullName, message.BirthDate, document);

            var account = new Acc(customer);

            var validator = new ValidateAccountValidator(account);
            if (!validator.IsValid)
            {
                var accountInvalidatedEvent = new AccountInvalidatedEvent(message.UserId, validator.GetErrors());

                await BusConfiguration
                    .BusEndpointInstance
                    .Publish(accountInvalidatedEvent)
                    .ConfigureAwait(false);
            }
        }
    }
}