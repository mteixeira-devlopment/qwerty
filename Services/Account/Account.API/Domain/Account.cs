using System;
using ServiceSeed.Actors;

namespace Account.API.Domain
{
    public sealed class Account : Entity
    {
        public Guid UserId { get; private set; }

        public Customer Customer { get; private set; }
        private Guid _customerId;

        public decimal Balance { get; private set; }

        private Account()
        {

        }

        public Account(Guid userId, Customer customer)
        {
            UserId = userId;

            Customer = customer;
            Balance = 0;
        }

        public void IncreaseBalance(decimal value)
        {
            Balance += value;
        }
    }
}