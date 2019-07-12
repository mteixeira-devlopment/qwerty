using System.Collections.Generic;
using System.Linq;
using ServiceSeed.Models;

namespace ServiceSeed.Handlers
{
    public sealed class NotificationHandler : INotificationHandler
    {
        private readonly List<Notification> _notifications;

        public NotificationHandler()
        {
            _notifications = new List<Notification>();
        }

        public void Notify(string errorMessage)
            => Handle(new Notification(errorMessage));

        public bool HasNotifications() => _notifications.Any();

        public Notification GetFirst() => _notifications.First();

        public List<Notification> GetNotifications() => _notifications;

        public List<string> GetNotificationErrors() 
            => _notifications.Select(n => n.ErrorMessage).ToList();

        private void Handle(Notification message) => _notifications.Add(message);
    }
}