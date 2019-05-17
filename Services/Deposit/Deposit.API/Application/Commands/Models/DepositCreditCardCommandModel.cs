using System;
using MediatR;
using SharedKernel.Responses;

namespace Deposit.API.Application.Commands.Models
{
    public class CreateChargeCommandModel : IRequest<CommandResponse>
    {
        public int ChargeId { get; private set; }
        public int Value { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public CreateChargeCommandModel(int chargeId, int value, DateTime createdAt)
        {
            ChargeId = chargeId;
            Value = value;
            CreatedAt = createdAt;
        }
    }

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
