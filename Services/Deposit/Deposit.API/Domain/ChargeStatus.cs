using ServiceSeed.Actors;

namespace Deposit.API.Domain
{
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