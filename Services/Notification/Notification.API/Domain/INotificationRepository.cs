using System.Threading.Tasks;

namespace Notification.API.Domain
{
    public interface INotificationRepository
    {
        Task<NotificationUser> CreateNotificationUserAsync(NotificationUser notificationUser);
        Task Commit();
    }
}