using System;
using System.Threading.Tasks;
using SharedKernel.Seed;

namespace Deposit.API.Domain
{
    public class Deposit : Entity
    {
        public decimal Value { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }
        private Guid _paymentMethod;

        public Deposit(decimal value, PaymentMethod paymentMethod)
        {
            Value = value;
            PaymentMethod = paymentMethod;
        }
    }

    public class PaymentMethod : Entity
    {
        public Guid AccountId { get; private set; }

        public PaymentMethod(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class CreditCard : PaymentMethod
    {
        public string Name { get; private set; }
        public string Number { get; private set; }
        public int ExpirationYear { get; private set; }
        public int ExpirationMonth { get; private set; }
        public string SecurityNumber { get; private set; }

        public CreditCard(Guid accountId, string name, string number, int expirationYear, int expirationMonth, string securityNumber)
            : base(accountId)
        {
            Name = name;
            Number = number;
            ExpirationYear = expirationYear;
            ExpirationMonth = expirationMonth;
            SecurityNumber = securityNumber;
        }
    }

    public interface IPayRepository
    {
        Task PayCreditCard(Deposit deposit, string creditCardMask, string paymentToken);
    }


}
