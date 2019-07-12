using System;
using ServiceSeed.Actors;

namespace Deposit.API.Domain
{
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
}
