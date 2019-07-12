using System.Collections.Generic;
using ServiceSeed.Models;

namespace ServiceSeed.Handlers
{
    public interface INotificationHandler
    {
        Notification GetFirst();
        List<Notification> GetNotifications();
        List<string> GetNotificationErrors();
        void Notify(string errorMessage);
        bool HasNotifications();
    }
}