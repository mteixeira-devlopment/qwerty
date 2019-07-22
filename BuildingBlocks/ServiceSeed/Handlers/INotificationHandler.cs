using System.Collections.Generic;
using ServiceSeed.Models;

namespace ServiceSeed.Handlers
{
    public interface INotificationHandler
    {
        Notification GetFirst();
        IEnumerable<string> GetFailNotification();
        bool HasFailNotifications();
        void NotifyInformation(string message);
        void NotifyWarning(string message);
        void NotifyFail(string message);
        void NotifyNotExpected(string message, string stackTrace);
        IEnumerable<Notification> GetNotifications();
        string GetNotExpectedNotification();
    }
}