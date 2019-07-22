using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using ServiceSeed.Commands;
using ServiceSeed.Responses;
using ServiceSeed.Validations;
using Acc = Account.API.Domain.Account;

namespace Account.API.Domain.Commands.IncreaseBalance
{
    public class IncreaseBalanceCommandHandler : CommandHandler<IncreaseBalanceCommandModel>
    {
        private readonly IEventBus _eventBus;
        private readonly IAccountRepository _accountRepository;

        public IncreaseBalanceCommandHandler(
            IEventBus eventBus,
            IAccountRepository accountRepository)
        {
            _eventBus = eventBus;
            _accountRepository = accountRepository;
        }

        public override async Task<CommandResponse> HandleCommand(IncreaseBalanceCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid(request, cancellationToken).ConfigureAwait(false);
            if (!validModel) return ReplyFlowFailure();

            var account = await GetAccount(request.DepositId, request.AccountId, cancellationToken).ConfigureAwait(false);
            if (account == null) return ReplyFlowFailure();

            account.IncreaseBalance(request.Value);
            await _accountRepository.UpdateBalance(account).ConfigureAwait(false);

            var committed = await _accountRepository.Commit().ConfigureAwait(false);
            if (!committed)
            {
                var errorMessage = $"Não possível salvar as informações de aumento de depósito. [{request.DepositId}]";
                var errorMessages = new List<string> { errorMessage };

                await PublishIncreaseBalanceInvalidatedIntegrationEvent(request.DepositId, errorMessages, cancellationToken);
                throw new Exception(errorMessage);
            }

            await PublishBalanceInscreasedIntegrationEvent(account.UserId, request.Value, cancellationToken);

            return ReplySuccessful();
        }

        private async Task<bool> CheckIfModelIsValid(IncreaseBalanceCommandModel requestModel, CancellationToken cancellationToken)
        {
            var validator = new CommandValidator<IncreaseBalanceCommandModel, IncreaseBalanceCommandValidator>(requestModel);

            if (validator.IsValid) return await Task.FromResult(true);

            var increaseBalanceInvalidatedIntegrationEvent = new IncreaseBalanceInvalidatedIntegrationEvent(
                requestModel.AccountId, validator.Errors);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(increaseBalanceInvalidatedIntegrationEvent), cancellationToken);
            #pragma warning restore 4014

            return await Task.FromResult(false);
        }

        private async Task<Acc> GetAccount(Guid depositId, Guid accountId, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.Get(accountId);
            if (account != null) return account;

            var errorMessage = $"Não foi possível encontrar uma conta com o identificador {accountId}";
            var errorMessages = new List<string> { errorMessage };

            await PublishIncreaseBalanceInvalidatedIntegrationEvent(depositId, errorMessages, cancellationToken);

            return null;
        }

        private Task PublishBalanceInscreasedIntegrationEvent(
            Guid userId, decimal value, CancellationToken cancellationToken)
        {
            var increasedBalanceIntegrationEvent = new BalanceIncreasedIntegrationEvent(userId, value);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(increasedBalanceIntegrationEvent), cancellationToken);

            return Task.CompletedTask;
        }

        private Task PublishIncreaseBalanceInvalidatedIntegrationEvent(
            Guid depositId, ICollection<string> errorMessages, CancellationToken cancellationToken)
        {
            var increaseBalanceInvalidatedIntegrationEvent 
                = new IncreaseBalanceInvalidatedIntegrationEvent(depositId, errorMessages);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(increaseBalanceInvalidatedIntegrationEvent), cancellationToken);

            return Task.CompletedTask;
        }
    }
}