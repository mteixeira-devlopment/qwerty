using FluentValidation;

namespace Deposit.API.Domain.Commands.DepositCreditCard
{
    public class DepositCreditCardCommandValidator : AbstractValidator<DepositCreditCardCommandModel>
    {
        public DepositCreditCardCommandValidator()
        {
            RuleFor(model => model.AccountId).NotEmpty().WithMessage("Identificador de conta não fornecido!");
        }
    }
}