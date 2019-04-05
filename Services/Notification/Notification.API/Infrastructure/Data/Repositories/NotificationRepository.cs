using System.Threading.Tasks;
using Notification.API.Domain;

using Not = Notification.API.Domain.Notification;

namespace Notification.API.Infrastructure.Data.Repositories
{
    public sealed class NotificationRepository : INotificationRepository
    {
        private readonly NotificationContext _notificationContext;

        public NotificationRepository(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public async Task<NotificationUser> CreateNotificationUserAsync(NotificationUser notificationUser)
        {
            await _notificationContext.AddAsync(notificationUser);
            return notificationUser;
        }

        public async Task Commit()
        {
            await _notificationContext.SaveChangesAsync();
        }
    }
}