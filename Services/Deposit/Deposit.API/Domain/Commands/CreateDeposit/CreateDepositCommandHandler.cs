﻿using System.Threading;
using System.Threading.Tasks;
using ServiceSeed.Commands;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;
using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Domain.Commands.CreateDeposit
{
    public class CreateDepositCommandHandler : CommandHandler<CreateDepositCommandModel>
    {
        private readonly IDepositRepository _depositRepository;

        public CreateDepositCommandHandler(
            INotificationHandler notificationHandler, 
            IDepositRepository depositRepository) : base(notificationHandler)
        {
            _depositRepository = depositRepository;
        }

        public override async Task<CommandResponse> HandleCommand(CreateDepositCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<CreateDepositCommandValidator>(request);
            if (!validModel) return ReplyFlowFailure();

            var chargeValue = await TryTransformIntToDecimal(request.Value);
            if (chargeValue == 0M) return ReplyFlowFailure();

            var charge = new Charge(request.ProviderChargeId, chargeValue, request.CreatedAt);
            var deposit = new Depos(request.AccountId, charge);

            await _depositRepository.CreateAsync(deposit);
            await _depositRepository.Commit();

            return ReplySuccessful(deposit.Id);
        }

        private async Task<decimal> TryTransformIntToDecimal(int value)
        {
            var stringValue = value.ToString();
            var stringValueWithDot = stringValue.Insert(stringValue.Length - 2, ",");

            var parsed = decimal.TryParse(stringValueWithDot, out var parsedValue);
            if (!parsed)
            {
                NotificationHandler.NotifyFail("Erro ao converter valor da transação!");
                return 0M;
            };

            return await Task.FromResult(parsedValue);
        }
    }
}