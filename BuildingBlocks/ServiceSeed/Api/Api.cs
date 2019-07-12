using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;

namespace ServiceSeed.Api
{
    public abstract class Api : ControllerBase
    {
        protected readonly INotificationHandler NotificationHandler;

        protected Api(INotificationHandler notificationHandler)
            => NotificationHandler = notificationHandler;

        protected IActionResult ReplyQuery()
            => NoContent();

        protected IActionResult ReplyQuery(object items)
            => Ok(items);

        protected IActionResult Reply(object result)
            => IsValidRequest()
                ? Ok(new SuccessResponse(result))
                : Reply();

        protected IActionResult Reply()
        {
            var notifications = NotificationHandler
                .GetNotifications();

            var errors = notifications
                .Select(n => n.ErrorMessage)
                .ToList();

            return UnprocessableEntity(new ErrorResponse(errors, 422));
        }

        protected IActionResult ReplyCreated(object result)
        {
            var requestPath = Request.Path;
            var uri = requestPath.Value;

            return Created(uri, new SuccessResponse(result));
        }

        protected IActionResult ReplyBadRequest(string requestError)
        {
            const int statusCode = 400;

            var errors = new List<string> { requestError };
            return BadRequest(new ErrorResponse(errors, statusCode));
        }

        protected IActionResult ReplyUnprocessableEntity(List<string> errors)
            => UnprocessableEntity(new ErrorResponse(errors, 422));

        private bool IsValidRequest()
            => !NotificationHandler.HasNotifications();
    }
}