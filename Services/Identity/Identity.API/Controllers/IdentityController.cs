using System.Threading.Tasks;
using Identity.API.Handlers;
using Identity.API.Services;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public IdentityController(
            INotificationHandler notificationHandler,
            IAuthenticationService authenticationService) : base(notificationHandler)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SigInUserAsync([FromBody]SignInUser model)
        {
            var token = await _authenticationService.SignInUserAsync(model);
            return OkResponse(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUpUserAsync(NewUser model)
        {
            await _authenticationService.SignUpUserAsync(model);
            return OkResponse();
        }
    }
}