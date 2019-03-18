using System.Collections.Generic;
using System.Linq;
using Identity.API.SharedKernel.Handlers;
using Identity.API.SharedKernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public abstract class ApiController : Controller
    {
        protected readonly IDomainNotificationHandler DomainNotificationHandler;

        protected ApiController(IDomainNotificationHandler domainNotificationHandler)
        {
            DomainNotificationHandler = domainNotificationHandler;
        }

        protected IActionResult OkResponse(object result = null)
        {
            if (IsValidRequest())
                return Ok(new SuccessResponse(result));

            var errors = DomainNotificationHandler
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
            return !DomainNotificationHandler.HasNotifications();
        }
    }
}