using System.Collections.Generic;
using FluentValidation.Results;
using Identity.API.Handlers;
using Identity.API.Models;

namespace Identity.API.Services
{
    public abstract class NotificationService
    {
        private readonly INotificationHandler _notificationHandler;

        protected NotificationService(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        protected void NotifyWithValidation(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                _notificationHandler.Handle(
                    new Notification(error.ErrorCode, error.ErrorMessage));
        }

        protected void NotifyWithError(string erroCode, string errorMessage)
            => _notificationHandler.Handle(new Notification(erroCode, errorMessage));

        protected void NotifyWithError(string errorMessage)
            => _notificationHandler.Handle(new Notification(errorMessage));

        protected bool HasNotifications() 
            => _notificationHandler.HasNotifications();

        protected IEnumerable<Notification> GetNotifications() 
            => _notificationHandler.GetNotifications();
    }
}
