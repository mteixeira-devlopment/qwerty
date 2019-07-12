using System;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Application.IntegrationEvents.Events;
using Notification.API.Domain;
using Notification.API.Hubs;

using Not = Notification.API.Domain.Notification;

namespace Notification.API.Application.IntegrationEvents.EventHandlers
{
    public class AccountCreatedIntegrationEventHandler : IIntegrationEventHandler<AccountCreatedIntegrationEvent>
    {
        private readonly IHubContext<RequestResponseMessageHub, IRequestResponseMessageHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;

        public AccountCreatedIntegrationEventHandler(
            IHubContext<RequestResponseMessageHub, IRequestResponseMessageHub> hubContext, 
            INotificationRepository notificationRepository)
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(AccountCreatedIntegrationEvent @event)
        {
            var notification = new Not(
                        "Conta criada com sucesso!",
                        "Parabéns, você concluiu seu cadastro com sucesso! " +
                        "Deposite um valor inicial e comece os trabalhos hoje mesmo :)");

            var notificationUser = new NotificationUser(@event.UserId, notification);
            notificationUser.Send();

            await _notificationRepository.CreateNotificationUserAsync(notificationUser);
            await _notificationRepository.Commit();

            await _hubContext.Clients.All.Notify($"Conta {@event.AccountNumber} criada com sucesso!");
        }

        public void Handle2()
        {
            throw new NotImplementedException();
        }
    }
}