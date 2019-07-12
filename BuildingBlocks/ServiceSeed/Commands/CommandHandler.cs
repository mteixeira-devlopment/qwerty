using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
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

        public CommandResponse ReplySuccessful(object content = null) => new CommandResponse(true, content);

        public CommandResponse ReplyFailure() => new CommandResponse(false);

        public async Task<CommandResponse> Handle(
            TModel request, 
            CancellationToken cancellationToken = default(CancellationToken))
            => await HandleCommand(request, cancellationToken);

        public abstract Task<CommandResponse> HandleCommand(TModel request, CancellationToken cancellationToken);

        protected async Task<bool> CheckIfModelIsValid<TValidator>(TModel requestModel)
            where TValidator : AbstractValidator<TModel>
        {
            var validator = new CommandValidator<TModel, TValidator>(requestModel);

            if (validator.IsValid) return await Task.FromResult(true);

            foreach (var error in validator.Errors)
                NotificationHandler.Notify(error);

            return await Task.FromResult(false);
        }
    }
}