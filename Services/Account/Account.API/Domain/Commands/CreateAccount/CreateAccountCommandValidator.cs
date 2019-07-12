using System;
using FluentValidation;

namespace Account.API.Domain.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommandModel>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(command => command.FullName).MinimumLength(3).WithMessage("O nome fornecido é muito curto.");

            RuleFor(command => command.BirthDate)
                .Must(birthday => DateTime.Now.AddYears(-18) >= birthday)
                .WithMessage("É necessário ter 18 anos ou mais para se cadastrar.");

            RuleFor(command => command.Document).Length(11).WithMessage("O documento não está no padrão correto.");
        }
    }
}