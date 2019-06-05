using System;
using MediatR;
using SharedKernel.Responses;

namespace Deposit.API.Application.Commands.Models
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