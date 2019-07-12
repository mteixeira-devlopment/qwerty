using System;
using ServiceSeed.Actors;

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

        public void Cancel() => Status = ChargeStatus.Canceled.Id;
        

    }
}