using System.Collections.Generic;
using Identity.API.Models;

namespace Identity.API.Handlers
{
    public interface INotificationHandler
    {
        List<Notification> GetNotifications();
        void Handle(Notification message);
        bool HasNotifications();
    }
}