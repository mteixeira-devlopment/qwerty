using System;
using MediatR;
using ServiceSeed.Responses;

namespace Account.API.Domain.Commands.IncreaseBalance
{
    public class IncreaseBalanceCommandModel : IRequest<CommandResponse>
    {
        public Guid DepositId { get; private set; }

        public Guid AccountId { get; private set; }
        public decimal Value { get; private set; }

        public IncreaseBalanceCommandModel(Guid depositId, Guid accountId, decimal value)
        {
            DepositId = depositId;

            AccountId = accountId;
            Value = value;
        }
    }
}