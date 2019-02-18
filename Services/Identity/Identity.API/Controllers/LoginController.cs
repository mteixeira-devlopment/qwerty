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
    public class LoginController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(
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
        public async Task<IActionResult> SiginUser([FromBody]SignInUser model)
        {
            var token = await _authenticationService.SignInUser(model);
            return OkResponse(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateDefaultUserAsync(NewUser model)
        {
            await _authenticationService.CreateUser(model);
            return OkResponse();
        }
    }
}