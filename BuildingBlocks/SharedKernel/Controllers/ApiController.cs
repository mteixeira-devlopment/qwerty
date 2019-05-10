using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Handlers;
using SharedKernel.Responses;

namespace SharedKernel.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected readonly INotificationHandler NotificationHandler;

        protected ApiController(INotificationHandler notificationHandler)
        {
            NotificationHandler = notificationHandler;
        }

        protected IActionResult ReplyOk(object items = null)
        {
            return Ok(
                items == null 
                    ? new QueryResponse(false) 
                    : new QueryResponse(true, items));
        }

        protected IActionResult Reply(object result = null)
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
            const int statusCode = 400;

            var errors = new List<string> { requestError };
            return BadRequest(new ErrorResponse(errors, statusCode));
        }

        protected IActionResult ReplyUnprocessableEntity(List<string> errors)
        {
            return UnprocessableEntity(new ErrorResponse(errors, 422));
        }

        private bool IsValidRequest()
        {
            return !NotificationHandler.HasNotifications();
        }
    }
}