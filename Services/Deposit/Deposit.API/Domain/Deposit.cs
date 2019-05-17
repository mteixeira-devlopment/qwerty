using System;
using System.Threading.Tasks;
using Deposit.API.Domain.DataTransferObjects;
using SharedKernel.Seed;

namespace Deposit.API.Domain
{
    public class Charge : Entity
    {
        public int ChargeId { get; private set; }
        public int Value { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public int Status { get; private set; }

        public Charge(int chargeId, int value, DateTime createdAt)
        {
            ChargeId = chargeId;
            Value = value;
            CreatedAt = createdAt;

            Status = ChargeStatus.New.Id;
        }

    }

    public class Deposit : Entity
    {
        public Charge Charge { get; private set; }
        private Guid _chargeId;

        public Payment PaymentMethod { get; private set; }
        private Guid _paymentMethod;

        public Deposit(Charge charge, Payment paymentMethod)
        {
            Charge = charge;
            PaymentMethod = paymentMethod;
        }
    }

    public class Payment : Entity
    {
        public Guid AccountId { get; private set; }

        public Payment(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class CreditCard : Payment
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
        Task<ChargeTransferObject.Data> CreateCharge(decimal value);
        Task<PaymentTransferObject.Data> PayCreditCard(int chargeId, string paymentToken);
    }

    public interface IDepositRepository
    {
        Task<Charge> CreateCharge(Charge charge);
    }

    public class ChargeStatus : Enumeration<ChargeStatus>
    {
        public static ChargeStatus New = new ChargeStatus(1, "New", "new");
        public static ChargeStatus Waiting = new ChargeStatus(2, "Waiting", "waiting");
        public static ChargeStatus Paid = new ChargeStatus(3, "Paid", "paid");
        public static ChargeStatus Unpaid = new ChargeStatus(4, "Unpaid", "unpaid");
        public static ChargeStatus Refunded = new ChargeStatus(5, "Refunded", "refunded");
        public static ChargeStatus Contested = new ChargeStatus(6, "Contested", "contested");
        public static ChargeStatus Canceled = new ChargeStatus(7, "Canceled", "canceled");
        public static ChargeStatus Settled = new ChargeStatus(8, "Settled", "settled");
        public static ChargeStatus Link = new ChargeStatus(9, "Link", "link");
        public static ChargeStatus Expired = new ChargeStatus(10, "Expired", "expired");

        public string ProviderName { get; private set; }

        protected ChargeStatus()
        {

        }

        public ChargeStatus(int id, string name, string providerName)
            : base(id, name)
        {
            ProviderName = providerName;
        }
    }
}
