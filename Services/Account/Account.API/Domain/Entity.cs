using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Results;

namespace Account.API.Domain
{
    public class Teste
    {
        public Teste()
        {
            var customer = new Customer("", "", DateTime.Now);
            var customerValidation = new Customer.CustomerValidation(customer);
            var isValid = customerValidation.IsValid;
        }
    }

    public sealed class Customer : Entity
    {
        public string FullName { get; private set; }
        public Document Document { get; private set; }
        public DateTime BirthDate { get; private set; }

        public Customer(string fullName, string document, DateTime birthDate)
        {
            FullName = fullName;
            Document = new Document(document);
            BirthDate = birthDate;
        }

        public sealed class CustomerValidation : Validation<Customer>
        {
            public CustomerValidation(Customer customer) : base(customer)
            {
                ValidateBirthDate();
                ValidateDocument();

                Validate();
            }

            private void ValidateBirthDate()
            {
               
            }

            private void ValidateDocument()
            {

            }
        }
    }

    public class Document : ValueObject
    {
        public string Text { get; private set; }
        public bool Verified { get; private set; }
        public string Photo { get; private set; }

        public Document(string text)
        {
            Text = text;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            yield return Verified;
            yield return Photo;
        }
    }

    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }

    public abstract class Validation<TEntity> : AbstractValidator<TEntity> where TEntity : Entity
    {
        private readonly TEntity _parent;
        private ValidationResult _validationResult;

        public bool IsValid { get; private set; }

        protected Validation(TEntity parent)
        {
            _parent = parent;
        }

        protected void Validate()
        {
            _validationResult = base.Validate(_parent);
            IsValid = _validationResult.IsValid;
        }

        public IEnumerable<string> GetErrors()
        {
            return _validationResult.Errors.Select(x => x.ErrorMessage);
        }


    }
}
