using System.Collections.Generic;
using System.Linq;
using Identity.API.Domain.Handlers;
using Identity.API.SharedKernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public abstract class ApiController : Controller
    {
        protected readonly INotificationHandler NotificationHandler;

        protected ApiController(INotificationHandler notificationHandler)
        {
            NotificationHandler = notificationHandler;
        }

        protected IActionResult ReplyOkOrUnprocessable(object result = null)
        {
            if (IsValidRequest())
                return Ok(new SuccessResponse(result));

            var errors = NotificationHandler
                .GetNotifications()
                .Select(n => n.ErrorMessage)
                .ToList();

            return UnprocessableEntity(new ErrorResponse(errors, 422));
        }

        protected IActionResult ReplyCreated(object result)
        {
            var uri = Request.Path.Value;
            return Created(uri, new SuccessResponse(result));
        }

        protected IActionResult ReplyBadRequest(string requestError)
        {
            var errors = new List<string> { requestError };
            var responseStatusCode = 400;

            return BadRequest(new ErrorResponse(errors, responseStatusCode));
        }

        private bool IsValidRequest()
        {
            return !NotificationHandler.HasNotifications();
        }
    }
}