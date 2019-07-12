using System;
using MediatR;
using ServiceSeed.Responses;

namespace Deposit.API.Domain.Commands.CreateDeposit
{
    public class CreateDepositCommandModel : IRequest<CommandResponse>
    {
        public Guid AccountId { get; private set; }
        public int ProviderChargeId { get; private set; }
        public int Value { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public CreateDepositCommandModel(Guid accountId, int providerChargeId, int value, DateTime createdAt)
        {
            AccountId = accountId;

            ProviderChargeId = providerChargeId;
            Value = value;
            CreatedAt = createdAt;
        }
    }
}