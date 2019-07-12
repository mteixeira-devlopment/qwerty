using System;
using MediatR;
using ServiceSeed.Responses;

namespace Deposit.API.Domain.Commands.DepositCreditCard
{
    public class DepositCreditCardCommandModel : IRequest<CommandResponse>
    {
        public Guid AccountId { get; private set; }

        public decimal Value { get; private set; }
        public string PaymentToken { get; private set; }

        public DepositCreditCardCommandModel(
            Guid accountId,
            decimal value,
            string paymentToken)
        {
            AccountId = accountId;
            Value = value;
            PaymentToken = paymentToken;
        }
    }
}
