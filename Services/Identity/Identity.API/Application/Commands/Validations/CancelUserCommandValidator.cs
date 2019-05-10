using FluentValidation;
using Identity.API.Application.Commands.Models;

namespace Identity.API.Application.Commands.Validations
{
    public sealed class CancelUserCommandValidator : AbstractValidator<CancelUserCommandModel>
    {
        public CancelUserCommandValidator()
        {
            RuleFor(command => command.UserId).NotNull().WithMessage("Identificador de usuário não fornecida!");
        }
    }
}