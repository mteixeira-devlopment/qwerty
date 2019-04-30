using System.Collections.Generic;
using Identity.API.Domain.Models;

namespace Identity.API.Domain.Handlers
{
    public interface INotificationHandler
    {
        DomainNotification GetFirst();
        List<DomainNotification> GetNotifications();
        IEnumerable<string> GetNotificationErrors();
        void Notify(string errorMessage);
        bool HasNotifications();
    }
}