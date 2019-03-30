using System;
using Account.API.Domain.Seed;

namespace Account.API.Domain
{
    public sealed class Account : Entity
    {
        public Customer Customer { get; private set; }
        private Guid _customerId;

        private Account()
        {

        }

        public Account(Customer customer)
        {
            Customer = customer;
        }
    }
}