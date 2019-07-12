using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSeed.Api;
using ServiceSeed.Handlers;

namespace Deposit.API.Controllers
{
    [Route("transaction")]
    [ApiController]
    public class TransactionController : Api
    {
        public TransactionController(INotificationHandler notificationHandler) : base(notificationHandler)
        {
            
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("provider-notification")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ManagerTransaction([FromBody] string providerToken)
        {
            if (string.IsNullOrEmpty(providerToken))
                return ReplyBadRequest("Token não fornecido!");

            return await Task.FromResult(Ok(true));
        }
    }
}