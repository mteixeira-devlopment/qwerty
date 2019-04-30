using System.Collections.Generic;
using System.Linq;
using Identity.API.Domain.Handlers;
using Identity.API.SharedKernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public abstract class ApiController : Controller
    {
        protected readonly INotificationHandler DomainNotificationHandler;

        protected ApiController(INotificationHandler domainNotificationHandler)
        {
            DomainNotificationHandler = domainNotificationHandler;
        }

        protected IActionResult OkOrUnprocessableResponse(object result = null)
        {
            if (IsValidRequest())
                return Ok(new SuccessResponse(result));

            var errors = DomainNotificationHandler
                .GetNotifications()
                .Select(n => n.ErrorMessage)
                .ToList();

            return UnprocessableEntity(new ErrorResponse(errors, 422));
        }

        protected IActionResult CreatedResponse(object result)
        {
            var uri = Request.Path.Value;
            return Created(uri, new SuccessResponse(result));
        }

        protected IActionResult BadRequestResponse(string requestError)
        {
            var errors = new List<string> { requestError };
            var responseStatusCode = Response.StatusCode;

            return BadRequest(new ErrorResponse(errors, responseStatusCode));
        }

        private bool IsValidRequest()
        {
            return !DomainNotificationHandler.HasNotifications();
        }
    }
}