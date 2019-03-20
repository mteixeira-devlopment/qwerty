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

        private readonly ISignUpService _signUpService;

        public CreateUserAndAccountSaga(
            [FromServices] ISignUpService signUpService)
        {
            _signUpService = signUpService;
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
            var user = new User(message.Username);
            await _signUpService.SignUp(user, message.Password);
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