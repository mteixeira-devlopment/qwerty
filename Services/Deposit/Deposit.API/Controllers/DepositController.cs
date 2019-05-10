using System.Threading.Tasks;
using Deposit.API.Application.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Controllers;
using SharedKernel.Handlers;

namespace Deposit.API.Controllers
{
    [Route("deposit")]
    [ApiController]
    public class DepositController : ApiController
    {
        private readonly IMediator _mediator;

        public DepositController(
            INotificationHandler notificationHandler, 
            IMediator mediator) : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("credit-card")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> DepositCreditCard([FromBody] DepositCreditCardCommandModel model)
        {
            if (model == null)
                return ReplyBadRequest("É necessário o preenchimento dos dados!");

            var result = await _mediator.Send(model);
            return Reply(result);
        }
    }
}
