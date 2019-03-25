using System.Collections.Generic;
using Identity.API.SharedKernel.Models;

namespace Identity.API.SharedKernel.Handlers
{
    public interface IDomainNotificationHandler
    {
        List<DomainNotification> GetNotifications();
        DomainNotification GetFirst();
        void Notify(string errorMessage);
        bool HasNotifications();
    }
}