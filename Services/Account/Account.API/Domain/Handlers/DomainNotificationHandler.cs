using System.Collections.Generic;
using System.Linq;
using Account.API.Domain.Models;

namespace Account.API.Domain.Handlers
{
    public sealed class DomainNotificationHandler : IDomainNotificationHandler
    {
        private readonly List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Notify(string errorMessage)
            => Handle(new DomainNotification(errorMessage));

        public bool HasNotifications() => _notifications.Any();

        public DomainNotification GetFirst() => _notifications.First();

        public List<DomainNotification> GetNotifications() => _notifications;

        public IEnumerable<string> GetNotificationErrors()
            => _notifications.Select(n => n.ErrorMessage);

        private void Handle(DomainNotification message) => _notifications.Add(message);
    }
}