using FluentValidation;

namespace Account.API.Domain.Commands.IncreaseBalance
{
    public class IncreaseBalanceCommandValidator : AbstractValidator<IncreaseBalanceCommandModel>
    {
        public IncreaseBalanceCommandValidator()
        {
            RuleFor(command => command.AccountId).NotEmpty().WithMessage("Identificador da conta não informado.");
            RuleFor(command => command.Value)
                .NotEmpty()
                .WithMessage("Valor para incremento em conta não informado.")
                .GreaterThan(0)
                .WithMessage("Valor para incremento em conta deve ser maior que 0.");
        }
    }
}