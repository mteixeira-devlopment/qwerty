using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;
using ServiceSeed.Validations;

namespace ServiceSeed.Commands
{
    public abstract class CommandHandler<TCommandModel> : IRequestHandler<TCommandModel, CommandResponse> 
        where TCommandModel : IRequest<CommandResponse>
    {
        protected INotificationHandler NotificationHandler { get; set; }

        protected CommandHandler(INotificationHandler notificationHandler) => NotificationHandler = notificationHandler;

        public CommandResponse ReplySuccessful() 
            => new CommandResponse((int) CommandExecutionResponseTypes.SuccessfullyExecution);

        public CommandResponse ReplySuccessful(object content) 
            => new CommandResponse((int) CommandExecutionResponseTypes.SuccessfullyExecution, content);

        public CommandResponse ReplyFlowFailure() 
            => new CommandResponse((int) CommandExecutionResponseTypes.FlowFailure);

        public CommandResponse ReplyExecutionFailure() 
            => new CommandResponse((int) CommandExecutionResponseTypes.ExecutionFailure);

        public async Task<CommandResponse> Handle(
            TCommandModel request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var commandName = GetCommandName();
            NotificationHandler.NotifyInformation($"Executando comando {commandName}");

            try
            {
                return await HandleCommand(request, cancellationToken);
            }
            catch (Exception exception)
            {
                NotificationHandler.NotifyNotExpected(exception.Message, exception.StackTrace);
                return ReplyExecutionFailure();
            }
        }

        public abstract Task<CommandResponse> HandleCommand(TCommandModel request, CancellationToken cancellationToken);

        protected async Task<bool> CheckIfModelIsValid<TValidator>(TCommandModel requestModel)
            where TValidator : AbstractValidator<TCommandModel>
        {
            var validator = new CommandValidator<TCommandModel, TValidator>(requestModel);

            if (validator.IsValid) return await Task.FromResult(true);

            foreach (var error in validator.Errors)
                NotificationHandler.NotifyFail(error);

            return await Task.FromResult(false);
        }

        private string GetCommandName()
        {
            var commandFullName = typeof(TCommandModel).FullName;
            var splitedCommandFullName = commandFullName.Split(".");

            var commandName = splitedCommandFullName[splitedCommandFullName.Length - 2];

            return commandName;
        }
    }
}