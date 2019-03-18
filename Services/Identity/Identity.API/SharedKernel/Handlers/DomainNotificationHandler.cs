using System.Collections.Generic;
using Identity.API.SharedKernel.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Identity.API.SharedKernel.Handlers
{
    public sealed class DomainNotificationHandler : IDomainNotificationHandler
    {
        private readonly List<Notification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<Notification>();
        }

        public void NotifyWithError(string errorMessage)
            => Handle(new Notification(errorMessage));

        public bool HasNotifications() => _notifications.Any();

        public List<Notification> GetNotifications() => _notifications;

        private void Handle(Notification message) => _notifications.Add(message);
    }
}