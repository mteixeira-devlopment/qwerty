using System;
using MediatR;
using SharedKernel.Responses;

namespace Deposit.API.Application.Commands.Models
{
    public class DepositCreditCardCommandModel : IRequest<CommandResponse>
    {
        public decimal Value { get; private set; }

        public Guid AccountId { get; private set; }

        public string CreditCardName { get; private set; }
        public string CreditCardNumber { get; private set; }
        public int CreditCardExpirationYear { get; private set; }
        public int CreditCardExpirationMonth { get; private set; }
        public string CreditCardSecurityNumber { get; private set; }
        public string CreditCardMask { get; private set; }

        public string PaymentToken { get; private set; }

        public DepositCreditCardCommandModel(
            decimal value,
            Guid accountId, 
            string creditCardName, 
            string creditCardNumber, 
            int creditCardExpirationYear, 
            int creditCardExpirationMonth, 
            string creditCardSecurityNumber,
            string creditCardMask,
            string paymentToken)
        {
            Value = value;

            AccountId = accountId;

            CreditCardName = creditCardName;
            CreditCardNumber = creditCardNumber;
            CreditCardExpirationYear = creditCardExpirationYear;
            CreditCardExpirationMonth = creditCardExpirationMonth;
            CreditCardSecurityNumber = creditCardSecurityNumber;
            CreditCardMask = creditCardMask;

            PaymentToken = paymentToken;
        }
    }
}
