using System;
using System.Threading.Tasks;
using Bus.Commands;
using Bus.Events;
using Identity.API.Configurations;
using Identity.API.Data.Repositories;
using Identity.API.Domain;
using Identity.API.SharedKernel.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Identity.API.Bus.Sagas
{
    public class CreateUserAndAccountSaga : Saga<CreateUserAndAccountSagaData>,
        IAmStartedByMessages<ValidateUserCommand>,
        IHandleMessages<ValidateAccountCommand>,
        IHandleMessages<AccountValidatedEvent>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IUserRepository _userRepository;

        public CreateUserAndAccountSaga(
            [FromServices] IDomainNotificationHandler domainNotificationHandler,
            [FromServices] UserManager<User> userManager,
            [FromServices] SignInManager<User> signInManager,
            [FromServices] TokenConfigurations tokenConfigurations,
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
            _userRepository = userRepository;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CreateUserAndAccountSagaData> mapper)
        {
            mapper.ConfigureMapping<ValidateUserCommand>(message => message.UserId)
                .ToSaga(sagaData => sagaData.UserId);

            mapper.ConfigureMapping<ValidateAccountCommand>(message => message.UserId)
                .ToSaga(sagaData => sagaData.UserId);
        }

        public async Task Handle(ValidateUserCommand message, IMessageHandlerContext context)
        {
            var existingUser = await _userManager.FindByNameAsync(message.Username);
            if (existingUser != null)
            {
                //DomainNotificationHandler.NotifyWithError("Já existe um usuário com este identificador");
                //return OkResponse();
            }

            var applicationUser = new User(message.Username);

            var createResult = _userManager
                .CreateAsync(applicationUser, message.Password).Result;

            if (!createResult.Succeeded)
            {
                foreach (var createError in createResult.Errors)
                    // DomainNotificationHandler.NotifyWithError(createError.Description);
                    Console.WriteLine("A");
            }

            await _userRepository.Commit();
        }

        public async Task Handle(ValidateAccountCommand message, IMessageHandlerContext context)
        {
            await context.Send(message).ConfigureAwait(false);
        }

        public Task Handle(AccountValidatedEvent message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

    }
}