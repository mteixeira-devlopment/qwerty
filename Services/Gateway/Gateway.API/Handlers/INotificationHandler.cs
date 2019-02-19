using System.Collections.Generic;
using Gateway.API.Models;

namespace Gateway.API.Handlers
{
    public interface INotificationHandler
    {
        List<Notification> GetNotifications();
        void Handle(Notification message);
        bool HasNotifications();
    }
}