using System;
using System.Threading.Tasks;
using Bus.Commands;
using Bus.Events;
using Identity.API.Configurations;
using Identity.API.Domain;
using Identity.API.SharedKernel.Handlers;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Identity.API.Bus.Sagas
{
    public class CreateUserAndAccountSaga : Saga<CreateUserAndAccountSagaData>,
        IAmStartedByMessages<ValidateUserCommand>,
        IHandleMessages<AccountValidatedEvent>,
        IHandleMessages<AccountInvalidatedEvent>
    {
        private readonly IDomainNotificationHandler _domainNotificationHandler;
        private readonly ISignUpService _signUpService;

        public CreateUserAndAccountSaga(
            [FromServices] IDomainNotificationHandler domainNotificationHandler,
            [FromServices] ISignUpService signUpService)
        {
            _domainNotificationHandler = domainNotificationHandler;
            _signUpService = signUpService;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CreateUserAndAccountSagaData> mapper)
        {
            mapper.ConfigureMapping<ValidateUserCommand>(message => message.UserId)
                .ToSaga(sagaData => sagaData.UserId);
        }

        public async Task Handle(ValidateUserCommand message, IMessageHandlerContext context)
        {
            var user = new User(message.Username);
            var signUp = await _signUpService.SignUp(user, message.Password);

            if (!signUp)
            {
                await HandlerInvalidatedUser();
                return;
            }

            await HandlerAccountValidate(user.Id, message.FullName, message.BirthDate, message.Document);
        }

        private async Task HandlerInvalidatedUser()
        {
            var invalidateReason = _domainNotificationHandler.GetFirst().ErrorMessage;
            var userInvalidatedEvent = new UserInvalidatedEvent(invalidateReason);

            await BusConfiguration
                .BusEndpointInstance
                .Publish(userInvalidatedEvent)
                .ConfigureAwait(false);

            MarkAsComplete();
        }

        private async Task HandlerAccountValidate(Guid userId, string fullName, DateTime birthDate, string document)
        {
            var validateAccountCommand =
                new ValidateAccountCommand(userId, fullName, birthDate, document);

            await BusConfiguration
                .BusEndpointInstance
                .Send(validateAccountCommand)
                .ConfigureAwait(false);
        }

        public Task Handle(AccountValidatedEvent message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public Task Handle(AccountInvalidatedEvent message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}