using System.Threading.Tasks;
using Bus.Events;
using Notification.API.Domain;

using Not = Notification.API.Domain.Notification;

namespace Notification.API.Bus.Handlers
{
    //public class AccountValidatedHandler : IHandleMessages<AccountValidatedEvent>
    //{
    //    private readonly INotificationRepository _notificationRepository;

    //    public AccountValidatedHandler(INotificationRepository notificationRepository)
    //    {
    //        _notificationRepository = notificationRepository;
    //    }

    //    public async Task Handle(AccountValidatedEvent message, IMessageHandlerContext context)
    //    {
    //        var notification = new Not(
    //            "Conta criada com sucesso!", 
    //            "Parabéns, você concluiu seu cadastro com sucesso! Deposite um valor inicial e comece os trabalhos hoje mesmo :)");

    //        var notificationUser = new NotificationUser(message.UserId, notification);
    //        notificationUser.Send();

    //        await _notificationRepository.CreateNotificationUserAsync(notificationUser);
    //        await _notificationRepository.Commit();
    //    }
    //}
}