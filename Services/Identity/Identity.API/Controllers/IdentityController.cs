using System.Threading.Tasks;
using Identity.API.Domain.Commands;
using Identity.API.Domain.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : ApiController
    {
        private readonly IMediator _mediator;

        public IdentityController(
            INotificationHandler notificationHandler,
            IMediator mediator) : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUpUserAsync([FromBody] CreateUserCommandModel model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("signin")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> SigInUserAsync([FromBody]SignInUser model)
        //{
        //    var user = await CheckIfExistsAsync(model.Username);
        //    if (user == null)
        //        return OkOrUnprocessableResponse();

        //    var passwordIsChecked = await CheckPasswordAsync(user, model.Password);
        //    if (!passwordIsChecked)
        //        return OkOrUnprocessableResponse();

        //    var token = GenerateToken(user);

        //    return OkOrUnprocessableResponse(token);
        //}

        //private async Task<User> CheckIfExistsAsync(string userIdentity)
        //{
        //    var user = await _userManager.FindByNameAsync(userIdentity);

        //    if (user == null)
        //        DomainNotificationHandler.Notify("Nenhum usuário encontrado com este identificador");

        //    return user;
        //}

        //private async Task<bool> CheckPasswordAsync(User user, string password)
        //{
        //    var signInResult = await _signInManager
        //        .CheckPasswordSignInAsync(user, password, false);

        //    if (!signInResult.Succeeded)
        //        DomainNotificationHandler.Notify("Credencias inválidas");

        //    return signInResult.Succeeded;
        //}

        //private string GenerateToken(User user)
        //{
        //    var claimsIdentity = GetClaims(user);

        //    var signInDate = DateTime.Now;
        //    var expireDate = signInDate +
        //                     TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

        //    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        //    var securityToken = jwtSecurityTokenHandler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Issuer = _tokenConfigurations.Issuer,
        //        Audience = _tokenConfigurations.Audience,
        //        SigningCredentials = _signingConfigurations.SigningCredentials,
        //        Subject = claimsIdentity,
        //        NotBefore = signInDate,
        //        Expires = expireDate
        //    });

        //    var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        //    return token;
        //}

        //private ClaimsIdentity GetClaims(User user)
        //{
        //    var claimsIdentity = new ClaimsIdentity(
        //        new GenericIdentity(user.Username, "SignIn"),
        //        new[] {
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        //            new Claim("Identity", user.Id.ToString())
        //        }
        //    );

        //    return claimsIdentity;
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("signup")]
        //[ProducesResponseType(typeof(string), 201)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> SignUpUserAsync(SignUpUser model)
        //{
        //    var user = new User(model.Username);
        //    var signUp = await _signUpService.SignUp(user, model.Password);

        //    if (!signUp)
        //    {
        //        await PublishInvalidatedUser();
        //        return OkOrUnprocessableResponse(DomainNotificationHandler.GetFirst().ErrorMessage);
        //    }

        //    await SendValidateAccount(model, user);

        //    var returnMessage = $"Obrigado, {model.FullName}. Aguarde enquanto analisamos sua documentação :)";
        //    return CreatedResponse(returnMessage);
        //}

        //private async Task PublishInvalidatedUser()
        //{
        //    var errors = DomainNotificationHandler.GetNotificationErrors();
        //    var userInvalidatedEvent = new UserInvalidatedEvent(errors);

        //    await BusConfiguration.BusEndpointInstance
        //        .Publish(userInvalidatedEvent)
        //        .ConfigureAwait(false);
        //}

        //private async Task SendValidateAccount(SignUpUser model, User user)
        //{
        //    var validateAccountCommand =
        //        new ValidateAccountCommand(user.Id, model.FullName, model.BirthDate, model.Document);

        //    await BusConfiguration.BusEndpointInstance
        //        .Send(validateAccountCommand)
        //        .ConfigureAwait(false);
        //}
    }
}