using System.Collections.Generic;
using Identity.API.SharedKernel.Models;

namespace Identity.API.SharedKernel.Handlers
{
    public interface IDomainNotificationHandler
    {
        List<Notification> GetNotifications();
        void NotifyWithError(string errorMessage);
        bool HasNotifications();
    }
}