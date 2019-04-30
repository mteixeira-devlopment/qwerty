using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Account.API.Domain;
using Account.API.Domain.Validations;
using Bus.Commands;
using Bus.Events;

using Acc = Account.API.Domain.Account;

namespace Account.API.Bus.Handlers
{
    //public class ValidateAccountHandler : IHandleMessages<ValidateAccountCommand>
    //{
    //    private readonly IAccountRepository _accountRepository;

    //    public ValidateAccountHandler(IAccountRepository accountRepository)
    //    {
    //        _accountRepository = accountRepository;
    //    }

    //    public async Task Handle(ValidateAccountCommand message, IMessageHandlerContext context)
    //    {
    //        var document = new Document(message.Document);
    //        var customer = new Customer(message.FullName, message.BirthDate, document);

    //        var account = new Acc(message.UserId, customer);

    //        var validator = new ValidateAccountValidator(account);
    //        if (!validator.IsValid)
    //        {
    //            await PublishAccountInvalidated(message.UserId, validator.GetErrors());
    //            return;
    //        }

    //        await _accountRepository.CreateAsync(account);
    //        await _accountRepository.Commit();

    //        await PublishAccountValidated(message.UserId);
    //    }

    //    private async Task PublishAccountInvalidated(Guid userId, IEnumerable<string> errors)
    //    {
    //        var accountInvalidatedEvent = new AccountInvalidatedEvent(userId, errors);

    //        await BusConfiguration
    //            .BusEndpointInstance
    //            .Publish(accountInvalidatedEvent)
    //            .ConfigureAwait(false);
    //    }

    //    private async Task PublishAccountValidated(Guid userId)
    //    {
    //        var accounValidatedEvent = new AccountValidatedEvent(userId);

    //        await BusConfiguration
    //            .BusEndpointInstance
    //            .Publish(accounValidatedEvent)
    //            .ConfigureAwait(false);
    //    }
    //}
}