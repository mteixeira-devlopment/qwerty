using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Organization.API.Models;

namespace Organization.API.Handlers
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly List<Notification> _notifications;

        public NotificationHandler()
        {
            _notifications = new List<Notification>();
        }

        public virtual List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification message)
        {
            _notifications.Add(message);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro: {message.ErrorCode}: {message.ErrorMessage}");
        }

        public virtual bool HasNotifications()
        {
            return _notifications.Any();
        }
    }
}