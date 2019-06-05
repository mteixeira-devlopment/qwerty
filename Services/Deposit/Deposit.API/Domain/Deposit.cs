using System;
using System.Threading.Tasks;
using Deposit.API.Domain.DataTransferObjects;
using Deposit.API.Infrastructure.Data.ExternalRepositories;
using SharedKernel.Seed;

namespace Deposit.API.Domain
{
    public class Charge : Entity
    {
        public int ProviderChargeId { get; private set; }
        public decimal Value { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public int Status { get; private set; }

        protected Charge()
        {

        }

        public Charge(int chargeId, decimal value, DateTime createdAt)
        {
            ProviderChargeId = chargeId;
            Value = value;
            CreatedAt = createdAt;

            Status = ChargeStatus.New.Id;
        }

    }

    public class Deposit : Entity
    {
        public Guid AccountId { get; private set; }

        public Charge Charge { get; private set; }
        private Guid _chargeId;

        protected Deposit()
        {

        }

        public Deposit(Guid accountId, Charge charge)
        {
            AccountId = accountId;
            Charge = charge;
        }
    }

    public interface IPayExternalRepository
    {
        Task<ExternalResponse<ChargeTransferObject>> CreateCharge(ChargeBodyTransferObject chargeBody);
        Task<ExternalResponse<PaymentTransferObject>> PayCreditCard(int chargeId, PaymentCreditCardBodyTransferObject paymentBody);
    }

    public interface IDepositRepository
    {
        Task<Deposit> CreateAsync(Deposit deposit);
        Task Commit();
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
