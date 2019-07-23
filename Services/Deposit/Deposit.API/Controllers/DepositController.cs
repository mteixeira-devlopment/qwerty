using System.Threading.Tasks;
using Deposit.API.Domain.Commands.DepositCreditCard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSeed.Api;
using ServiceSeed.Commands;
using ServiceSeed.Handlers;

namespace Deposit.API.Controllers
{
    [Route("deposit")]
    [ApiController]
    public class DepositController : Api
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

            return result.ExecutionResult == (int) CommandExecutionResponseTypes.SuccessfullyExecution
                ? ReplyCreated(result.Content)
                : ReplyFailure(result.ExecutionResult);
        }
    }
}
