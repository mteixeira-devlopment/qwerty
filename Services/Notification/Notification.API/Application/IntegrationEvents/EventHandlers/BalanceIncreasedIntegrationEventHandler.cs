using System.Threading.Tasks;
using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Application.IntegrationEvents.Events;
using Notification.API.Domain;
using Notification.API.Hubs;

namespace Notification.API.Application.IntegrationEvents.EventHandlers
{
    public class BalanceIncreasedIntegrationEventHandler : IIntegrationEventHandler<BalanceIncreasedIntegrationEvent>
    {
        private readonly IHubContext<RequestResponseMessageHub, IRequestResponseMessageHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;

        public BalanceIncreasedIntegrationEventHandler(
            IHubContext<RequestResponseMessageHub, IRequestResponseMessageHub> hubContext,
            INotificationRepository notificationRepository)
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(BalanceIncreasedIntegrationEvent @event)
        {
            var notification = new Domain.Notification(
                "Depósito efetuado com sucesso!",
                $"Um depósito de R$ {@event.Value} foi efetuado em sua conta!");

            var notificationUser = new NotificationUser(@event.UserId, notification);
            notificationUser.Send();

            await _notificationRepository.CreateNotificationUserAsync(notificationUser);
            await _notificationRepository.Commit();

            await _hubContext.Clients.All.Notify($"Depósito no valor de R$ {@event.Value} efetuado com sucesso!");

            return true;
        }
    }
}