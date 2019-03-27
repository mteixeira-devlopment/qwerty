﻿using System.Collections.Generic;
using System.Linq;
using Account.API.SharedKernel.Models;

namespace Account.API.SharedKernel.Handlers
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

        public List<DomainNotification> GetNotifications() => _notifications;

        public DomainNotification GetFirst() => _notifications.First();

        private void Handle(DomainNotification message) => _notifications.Add(message);
    }
}