using FluentValidation;

namespace Deposit.API.Domain.Commands.CreateDeposit
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