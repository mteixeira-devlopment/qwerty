using FluentValidation;
using Identity.API.Application.Commands.Models;

namespace Identity.API.Application.Commands.Validations
{
    public sealed class SignInUserCommandValidator : AbstractValidator<SignInUserCommandModel>
    {
        public SignInUserCommandValidator()
        {
            RuleFor(command => command.Username).NotEmpty().WithMessage("O usuário é obrigatório!");
            RuleFor(command => command.Password).NotEmpty().WithMessage("A senha é obrigatória!");
        }
    }
}