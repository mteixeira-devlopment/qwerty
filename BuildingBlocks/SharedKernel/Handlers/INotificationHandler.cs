using System.Collections.Generic;
using SharedKernel.Models;

namespace SharedKernel.Handlers
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