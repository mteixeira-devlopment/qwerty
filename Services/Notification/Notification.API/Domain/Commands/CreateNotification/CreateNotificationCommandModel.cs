using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ServiceSeed.Commands;
using ServiceSeed.Responses;

namespace Notification.API.Domain.Commands.CreateNotification
{
    public class CreateNotificationCommandModel : IRequest<CommandResponse>
    {
        public Guid UserId { get; private set; }

        public string Title { get; private set; }
        public string Summary { get; private set; }

        public CreateNotificationCommandModel(Guid userId, string title, string summary)
        {
            UserId = userId;

            Title = title;
            Summary = summary;
        }
    }

    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommandModel>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(command => command.UserId).NotEmpty().WithMessage("Identificador de usuário não fornecido.");

            RuleFor(command => command.Title).NotEmpty().WithMessage("Não foi informado o título da notificação.");
            RuleFor(command => command.Summary).NotEmpty().WithMessage("Não foi informado o conteúdo da notificação.");
        }
    }

    public class CreateNotificationCommandHandler : CommandHandler<CreateNotificationCommandModel>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public override async Task<CommandResponse> HandleCommand(CreateNotificationCommandModel request, CancellationToken cancellationToken)
        {
            var notification = new Notification(request.Title, request.Summary);

            var notificationUser = new NotificationUser(request.UserId, notification);
            notificationUser.Send();

            await _notificationRepository.CreateNotificationUserAsync(notificationUser);
            await _notificationRepository.Commit();

            //await _hubContext.Clients.All.Notify($"Conta {@event.AccountNumber} criada com sucesso!");

            return ReplySuccessful();
        }
    }
}
