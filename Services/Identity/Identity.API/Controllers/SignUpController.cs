using System.Threading.Tasks;
using Identity.API.Domain;
using Identity.API.Domain.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSeed.Api;
using ServiceSeed.Commands;
using ServiceSeed.Handlers;

namespace Identity.API.Controllers
{
    [Route("signup")]
    [ApiController]
    public class SignUpController : Api
    {
        private readonly IMediator _mediator;

        public SignUpController(
            INotificationHandler notificationHandler,
            IMediator mediator) : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommandModel model)
        {
            if (model == null)
                return ReplyBadRequest("É necessário o preenchimento dos dados!");

            var result = await _mediator.Send(model);

            return result.ExecutionResult == (int) CommandExecutionResponseTypes.SuccessfullyExecution 
                ? ReplyCreated(result.Content) 
                : ReplyFailure(result.ExecutionResult);
        }
    }
}