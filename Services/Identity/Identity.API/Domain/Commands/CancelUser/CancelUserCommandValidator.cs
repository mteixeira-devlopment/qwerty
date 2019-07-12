using FluentValidation;

namespace Identity.API.Domain.Commands.CancelUser
{
    public sealed class CancelUserCommandValidator : AbstractValidator<CancelUserCommandModel>
    {
        public CancelUserCommandValidator()
        {
            RuleFor(command => command.UserId).NotEmpty().WithMessage("Identificador de usuário não fornecido!");
        }
    }
}