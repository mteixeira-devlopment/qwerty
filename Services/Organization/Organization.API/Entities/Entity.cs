using System;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;

namespace Organization.API.Entities
{
    public abstract class Entity<TEntity> where TEntity : class
    {
        protected Entity()
        {
            ValidationResult = new ValidationResult();
        }

        public Guid Id { get; set; }

        [NotMapped]
        public ValidationResult ValidationResult { get; private set; }

        public virtual bool IsValid()
        {
            return true;
        }

        public void AddValidation(ValidationResult validationResult)
        {
            foreach (var validationResultError in validationResult.Errors)
                ValidationResult.Errors.Add(validationResultError);
        }

        public void AddError(string field, string error)
        {
            var failure = new ValidationFailure(field, error);
            ValidationResult.Errors.Add(failure);
        }
    }
}
