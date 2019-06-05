using Deposit.API.Application.Commands.Models;
using FluentValidation;

namespace Deposit.API.Application.Commands.Validations
{
    public class CreateDepositCommandValidator : AbstractValidator<CreateDepositCommandModel>
    {
        public CreateDepositCommandValidator()
        {
            RuleFor(model => model.ProviderChargeId).NotEmpty().WithMessage("Identificador da transação não fornecido!");
            RuleFor(model => model.Value).NotEmpty().WithMessage("O valor da transação não foi fornecido!");
            RuleFor(model => model.CreatedAt).NotEmpty()
                .WithMessage("A data de criação da transação não foi fornecida!");
        }
    }
}