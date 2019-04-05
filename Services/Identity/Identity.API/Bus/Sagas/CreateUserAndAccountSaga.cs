namespace Identity.API.Bus.Sagas
{
    //public class CreateUserAndAccountSaga : Saga<CreateUserAndAccountSagaData>,
    //    IAmStartedByMessages<ValidateUserCommand>,
    //    IHandleMessages<AccountValidatedEvent>,
    //    IHandleMessages<AccountInvalidatedEvent>
    //{
    //    private readonly IDomainNotificationHandler _domainNotificationHandler;
    //    private readonly ISignUpService _signUpService;

    //    public CreateUserAndAccountSaga(
    //        [FromServices] IDomainNotificationHandler domainNotificationHandler,
    //        [FromServices] ISignUpService signUpService)
    //    {
    //        _domainNotificationHandler = domainNotificationHandler;
    //        _signUpService = signUpService;
    //    }

    //    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CreateUserAndAccountSagaData> mapper)
    //    {
    //        mapper.ConfigureMapping<ValidateUserCommand>(message => message.UserId)
    //            .ToSaga(sagaData => sagaData.UserId);
    //    }

    //    public Task Handle(AccountValidatedEvent message, IMessageHandlerContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task Handle(AccountInvalidatedEvent message, IMessageHandlerContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}