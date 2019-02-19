using System.Collections.Generic;
using System.Linq;
using Gateway.API.Handlers;
using Gateway.API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers
{
    public abstract class ApiController : Controller
    {
        private readonly INotificationHandler _notificationHandler;

        protected ApiController(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler as NotificationHandler;
        }

        protected IActionResult OkResponse(object result = null)
        {
            if (IsValidRequest())
                return Ok(new SuccessResponse(result));

            var errors = _notificationHandler
                .GetNotifications()
                .Select(n => n.ErrorMessage)
                .ToList();

            return Ok(new ErrorResponse(errors, 200));
        }

        protected IActionResult BadRequestResponse(string requestError)
        {
            var errors = new List<string> { requestError };
            var responseStatusCode = Response.StatusCode;

            return BadRequest(new ErrorResponse(errors, responseStatusCode));
        }

        private bool IsValidRequest()
        {
            return !_notificationHandler.HasNotifications();
        }
    }
}