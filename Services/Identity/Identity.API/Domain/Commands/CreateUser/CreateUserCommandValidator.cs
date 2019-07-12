using FluentValidation;

namespace Identity.API.Domain.Commands.CreateUser
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommandModel>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.Username).NotEmpty().WithMessage("O usuário é obrigatório!");
            RuleFor(command => command.Username).MinimumLength(3).WithMessage("O usuário deve conter no mínimo 3 caracteres!");

            RuleFor(command => command.Password).NotEmpty().WithMessage("A senha é obrigatória!");
            RuleFor(command => command.Password).MinimumLength(6).WithMessage("A senha deve conter no mínimo 3 caracteres!");
        }
    }
}