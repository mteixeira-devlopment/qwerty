using System.Collections.Generic;
using Account.API.SharedKernel.Models;

namespace Account.API.SharedKernel.Handlers
{
    public interface IDomainNotificationHandler
    {
        List<DomainNotification> GetNotifications();
        DomainNotification GetFirst();
        void Notify(string errorMessage);
        bool HasNotifications();
    }
}