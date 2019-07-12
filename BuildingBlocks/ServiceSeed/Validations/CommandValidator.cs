using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MediatR;
using ServiceSeed.Responses;

namespace ServiceSeed.Validations
{
    public class CommandValidator<TModel, TValidator>
        where TModel : IRequest<CommandResponse>
        where TValidator : AbstractValidator<TModel>
    {
        public bool IsValid { get; private set; }
        public ICollection<string> Errors { get; private set; }

        public CommandValidator(TModel commandModel)
        {
            var validatorInstance = Activator.CreateInstance<TValidator>();
            var validationResults = validatorInstance.Validate(commandModel);

            IsValid = validationResults.IsValid;

            if (!IsValid)
                Errors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}
