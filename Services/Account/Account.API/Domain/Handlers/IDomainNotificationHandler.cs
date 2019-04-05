using System.Collections.Generic;
using Account.API.Domain.Models;

namespace Account.API.Domain.Handlers
{
    public interface IDomainNotificationHandler
    {
        List<DomainNotification> GetNotifications();
        DomainNotification GetFirst();
        void Notify(string errorMessage);
        bool HasNotifications();
    }
}