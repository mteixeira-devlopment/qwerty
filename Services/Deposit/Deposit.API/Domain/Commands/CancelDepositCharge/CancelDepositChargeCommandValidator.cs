using FluentValidation;

namespace Deposit.API.Domain.Commands.CancelDepositCharge
{
    public class CancelDepositChargeCommandValidator : AbstractValidator<CancelDepositChargeCommandModel>
    {
        public CancelDepositChargeCommandValidator()
        {
            RuleFor(command => command.DepositId).NotEmpty().WithMessage("Identificador de depósito não fornecido.");
            RuleFor(command => command.CancellationReasons).NotEmpty()
                .WithMessage("É necessário ao menos uma razão de cancelamento.");
        }
    }
}