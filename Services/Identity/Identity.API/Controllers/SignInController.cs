using System.Threading.Tasks;
using Identity.API.Domain.Commands.SignInUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSeed.Api;
using ServiceSeed.Handlers;

namespace Identity.API.Controllers
{
    [Route("signin")]
    [ApiController]
    public class SignInController : Api
    {
        private readonly IMediator _mediator;

        public SignInController(
            INotificationHandler notificationHandler,
            IMediator mediator) : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> SigInUserAsync([FromBody] SignInUserCommandModel model)
        {
            if (model == null)
                return ReplyBadRequest("É necessário o preenchimento dos dados!");

            var result = await _mediator.Send(model);
            return Reply(result);
        }
    }
}