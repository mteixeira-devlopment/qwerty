using System;
using Account.API.Domain.Seed;

namespace Account.API.Domain
{
    public sealed class Account : Entity
    {
        public Guid UserId { get; private set; }

        public Customer Customer { get; private set; }
        private Guid _customerId;

        private Account()
        {

        }

        public Account(Guid userId, Customer customer)
        {
            UserId = userId;

            Customer = customer;
        }
    }
}