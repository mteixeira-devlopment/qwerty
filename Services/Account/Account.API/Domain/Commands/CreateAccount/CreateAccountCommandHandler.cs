using System;
using System.Threading;
using System.Threading.Tasks;
using Account.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using ServiceSeed.Commands;
using ServiceSeed.Responses;
using ServiceSeed.Validations;

namespace Account.API.Domain.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : CommandHandler<CreateAccountCommandModel>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEventBus _eventBus;

        public CreateAccountCommandHandler(
            IAccountRepository accountRepository, 
            IEventBus eventBus)
        {
            _accountRepository = accountRepository;
            _eventBus = eventBus;
        }

        public override async Task<CommandResponse> HandleCommand(CreateAccountCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid(request, cancellationToken);
            if (!validModel) ReplyFlowFailure();

            var document = new Document(request.Document);
            var customer = new Customer(request.FullName, request.BirthDate, document);

            var account = new Account(request.UserId, customer);

            await _accountRepository.CreateAsync(account).ConfigureAwait(false);
            await _accountRepository.Commit().ConfigureAwait(false);

            await PublishAccountCreatedIntegrationEvent(request.UserId, cancellationToken);

            return ReplySuccessful(account);
        }

        private async Task<bool> CheckIfModelIsValid(CreateAccountCommandModel requestModel, CancellationToken cancellationToken)
        {
            var validator = new CommandValidator<CreateAccountCommandModel, CreateAccountCommandValidator>(requestModel);

            if (validator.IsValid) return await Task.FromResult(true);

            var accountInvalidatedIntegrationEvent = new AccountInvalidatedIntegrationEvent(
                requestModel.UserId, validator.Errors);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(accountInvalidatedIntegrationEvent), cancellationToken);

            return await Task.FromResult(false);
        }

        private Task PublishAccountCreatedIntegrationEvent(
            Guid userId, CancellationToken cancellationToken)
        {
            var accountCreatedIntegrationEvent = new AccountCreatedIntegrationEvent(userId, "1230568221-201");

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(accountCreatedIntegrationEvent), cancellationToken);

            return Task.CompletedTask;
        }
    }
}