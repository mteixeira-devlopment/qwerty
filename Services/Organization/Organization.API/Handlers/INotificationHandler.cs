using System.Collections.Generic;
using Organization.API.Models;

namespace Organization.API.Handlers
{
    public interface INotificationHandler
    {
        List<Notification> GetNotifications();
        void Handle(Notification message);
        bool HasNotifications();
    }
}