using System;
using ServiceSeed.Actors;

namespace Account.API.Domain
{
    public sealed class Customer : Entity
    {
        public string FullName { get; private set; }
        public DateTime BirthDate { get; private set; }

        public Document Document { get; private set; }

        private Customer()
        {
            
        }

        public Customer(string fullName, DateTime birthDate, Document document)
        {
            FullName = fullName;
            BirthDate = birthDate;

            Document = document;
        }
    }
}