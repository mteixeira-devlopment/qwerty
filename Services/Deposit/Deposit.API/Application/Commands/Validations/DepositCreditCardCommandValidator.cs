using Deposit.API.Application.Commands.Models;
using FluentValidation;

namespace Deposit.API.Application.Commands.Validations
{
    public class DepositCreditCardCommandValidator : AbstractValidator<DepositCreditCardCommandModel>
    {
        public DepositCreditCardCommandValidator()
        {
            RuleFor(model => model.AccountId).NotNull().WithMessage("Identificador de conta não fornecida!");
            RuleFor(model => model.CreditCardName).NotEmpty().WithMessage("É necessário o nome impresso no cartão de crédito!");
            RuleFor(model => model.CreditCardNumber).Length(16).WithMessage("O formato do cartão está incorreto!");
            RuleFor(model => model.CreditCardSecurityNumber).Length(3).WithMessage("O formato do código de segurança está incorreto!");
        }
    }
}