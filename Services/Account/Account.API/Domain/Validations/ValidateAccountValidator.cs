using System;
using FluentValidation;

namespace Account.API.Domain.Validations
{
    public class ValidateAccountValidator : AccountValidator
    {
        public ValidateAccountValidator(Account account) : base(account)
        {
            RuleFor(acc => account.Customer.FullName)
                .MinimumLength(3)
                .WithMessage("O nome fornecido é muito curto.");

            RuleFor(acc => account.Customer.BirthDate)
                .Must(birthday => DateTime.Now.AddYears(-18) >= birthday)
                .WithMessage("É necessário ter 18 anos ou mais para se cadastrar.");

            RuleFor(acc => account.Customer.Document)
                .Must(document => document.Text.Length == 18)
                .WithMessage("O documento não está no padrão correto.");

            Validate();
        }
    }
}