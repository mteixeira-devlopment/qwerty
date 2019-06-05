using Deposit.API.Application.Commands.Models;
using FluentValidation;

namespace Deposit.API.Application.Commands.Validations
{
    public class DepositCreditCardCommandValidator : AbstractValidator<DepositCreditCardCommandModel>
    {
        public DepositCreditCardCommandValidator()
        {
            RuleFor(model => model.AccountId).NotNull().WithMessage("Identificador de conta não fornecida!");
        }
    }
}