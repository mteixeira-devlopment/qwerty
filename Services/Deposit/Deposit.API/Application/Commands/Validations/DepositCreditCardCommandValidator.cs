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

    public class CreateChargeCommandValidator : AbstractValidator<CreateChargeCommandModel>
    {
        public CreateChargeCommandValidator()
        {
            RuleFor(model => model.ChargeId).NotEmpty().WithMessage("Identificador da transação não fornecido!");
            RuleFor(model => model.Value).NotEmpty().WithMessage("O valor da transação não foi fornecido!");
            RuleFor(model => model.CreatedAt).NotEmpty()
                .WithMessage("A data de criação da transação não foi fornecida!");
        }
    }
}