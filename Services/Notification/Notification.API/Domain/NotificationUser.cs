using System;
using SharedKernel.Seed;

namespace Notification.API.Domain
{
    public sealed class NotificationUser : Entity
    {
        public Guid UserId { get; private set; }

        public Notification Notification { get; private set; }
        private Guid _notificationId;

        public DateTime SentIn { get; private set; }

        private NotificationUser()
        {
            
        }

        public NotificationUser(Guid userId, Notification notification)
        {
            UserId = userId;

            Notification = notification;
        }

        public void Send()
        {
            SentIn = DateTime.UtcNow;
        }
    }
}