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

        public void NotifyInformation(string message)
            => Handle(new InfoNotification(message));

        public void NotifyWarning(string message)
            => Handle(new WarningNotification(message));

        public void NotifyFail(string message)
            => Handle(new FailNotification(message));

        public void NotifyNotExpected(string message, string stackTrace)
        {
            var notExpectedNotification = new NotExpectedNotification(message, stackTrace);

            var orderedEvents = _notifications
                .Select(not => not.Message);

            notExpectedNotification.AddExecutionTrace(orderedEvents.ToArray());

            Handle(notExpectedNotification);
        }

        public bool HasFailNotifications() => _notifications.Any(not => not.Severity == (int) Notification.NotificationSeverityTypes.Fail);

        public Notification GetFirst() => _notifications.First();

        public IEnumerable<Notification> GetNotifications() => _notifications;

        public IEnumerable<string> GetFailNotification()
        {
            return _notifications
                .Where(not => not.Severity == (int) Notification.NotificationSeverityTypes.Fail)
                .Select(not => not.Message);
        }

        public string GetNotExpectedNotification()
        {
            return _notifications
                .First(not => not.Severity == (int) Notification.NotificationSeverityTypes.NotExpected)
                .Message;
        }

        private void Handle(Notification notification) => _notifications.Add(notification);
    }
}