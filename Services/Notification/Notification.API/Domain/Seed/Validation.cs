using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Notification.API.Domain.Seed
{
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
            _validationResult = Validate(_parent);
            IsValid = _validationResult.IsValid;
        }

        public IEnumerable<string> GetErrors()
        {
            return _validationResult.Errors.Select(x => x.ErrorMessage);
        }
    }
}