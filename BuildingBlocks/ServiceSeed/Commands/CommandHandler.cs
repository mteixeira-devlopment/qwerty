using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;
using ServiceSeed.Validations;

namespace ServiceSeed.Commands
{
    public abstract class CommandHandler<TModel> : IRequestHandler<TModel, CommandResponse> 
        where TModel : IRequest<CommandResponse>
    {
        protected INotificationHandler NotificationHandler { get; set; }

        protected CommandHandler() { }

        protected CommandHandler(INotificationHandler notificationHandler) => NotificationHandler = notificationHandler;

        public CommandResponse ReplySuccessful() 
            => new CommandResponse((int) CommandExecutionResponseTypes.SuccessfullyExecution);

        public CommandResponse<TResponseContent> ReplySuccessful<TResponseContent>(TResponseContent content) 
            => new CommandResponse<TResponseContent>((int) CommandExecutionResponseTypes.SuccessfullyExecution, content);

        public CommandResponse ReplyFlowFailure() 
            => new CommandResponse((int) CommandExecutionResponseTypes.FlowFailure);

        public CommandResponse ReplyExecutionFailure() 
            => new CommandResponse((int) CommandExecutionResponseTypes.ExecutionFailure);

        public async Task<CommandResponse> Handle(
            TModel request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var commandName = typeof(TModel).FullName;

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

        public abstract Task<CommandResponse> HandleCommand(TModel request, CancellationToken cancellationToken);

        protected async Task<bool> CheckIfModelIsValid<TValidator>(TModel requestModel)
            where TValidator : AbstractValidator<TModel>
        {
            var validator = new CommandValidator<TModel, TValidator>(requestModel);

            if (validator.IsValid) return await Task.FromResult(true);

            foreach (var error in validator.Errors)
                NotificationHandler.NotifyFail(error);

            return await Task.FromResult(false);
        }
    }
}