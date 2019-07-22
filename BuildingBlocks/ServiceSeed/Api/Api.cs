using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ServiceSeed.Commands;
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

        protected IActionResult ReplyOk(object result)
        {
            return Ok(new SuccessResponse(result));
        }

        protected IActionResult ReplyCreated(object result)
        {
            var requestPath = Request.Path;
            var uri = requestPath.Value;

            return Created(uri, new SuccessResponse(result));
        }

        protected IActionResult ReplyBadRequest(string requestError)
        {
            var errors = new List<string> { requestError };
            return BadRequest(new ErrorResponse(errors, 400));
        }

        protected IActionResult ReplyFailure(int executionResult)
        {
            if (executionResult == (int) CommandExecutionResponseTypes.ExecutionFailure)
                return ReplyInternalServerError();

            var notifications = NotificationHandler
                .GetFailNotification();

            return ReplyUnprocessableEntity(notifications);
        }

        private IActionResult ReplyUnprocessableEntity(IEnumerable<string> errors)
            => UnprocessableEntity(new ErrorResponse(errors, 422));

        private IActionResult ReplyInternalServerError()
        {
            const int serverErrorStatus = (int) HttpStatusCode.InternalServerError;
            var serverErrorMessage = NotificationHandler.GetNotExpectedNotification();

            return StatusCode(serverErrorStatus, new ErrorResponse(serverErrorMessage, serverErrorStatus));
        }

        private bool IsValidExecution()
            => !NotificationHandler.HasFailNotifications();
    }
}