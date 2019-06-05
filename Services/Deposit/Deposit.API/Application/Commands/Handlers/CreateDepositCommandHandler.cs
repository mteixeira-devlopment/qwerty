using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.Commands.Models;
using Deposit.API.Application.Commands.Validations;
using Deposit.API.Domain;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;

using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Application.Commands.Handlers
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
            if (!validModel) return ReplyFailure();

            var chargeValue = await TryTransformIntToDecimal(request.Value);
            if (chargeValue == 0M) return ReplyFailure();

            var charge = new Charge(request.ProviderChargeId, chargeValue, request.CreatedAt);
            var deposit = new Depos(request.AccountId, charge);

            await _depositRepository.CreateAsync(deposit);
            await _depositRepository.Commit();

            return ReplySuccessful();
        }

        private async Task<decimal> TryTransformIntToDecimal(int value)
        {
            var stringValue = value.ToString();
            var stringValueWithDot = stringValue.Insert(stringValue.Length - 2, ",");

            var parsed = decimal.TryParse(stringValueWithDot, out var parsedValue);
            if (!parsed)
            {
                NotificationHandler.Notify("Erro ao converter valor da transação!");
                return 0M;
            };

            return await Task.FromResult(parsedValue);
        }
    }
}