using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Bus.Commands;
using Identity.API.Configurations;
using Identity.API.Data.Repositories;
using Identity.API.Domain;
using Identity.API.SharedKernel.Handlers;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NServiceBus;

namespace Identity.API.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : ApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IUserRepository _userRepository;

        public IdentityController(
            IDomainNotificationHandler domainNotificationHandler,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenConfigurations tokenConfigurations,
            SigningConfigurations signingConfigurations,
            IUserRepository userRepository) : base(domainNotificationHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SigInUserAsync([FromBody]SignInUser model)
        {
            var user = await CheckIfExistsAsync(model.UserIdentity);
            if (user == null)
                return OkResponse();

            var passwordIsChecked = await CheckPasswordAsync(user, model.Password);
            if (!passwordIsChecked)
                return OkResponse();

            var token = GenerateToken(user);
           
            return OkResponse(token);
        }

        private async Task<User> CheckIfExistsAsync(string userIdentity)
        {
            var user = await _userManager.FindByNameAsync(userIdentity);

            if (user == null)
                DomainNotificationHandler.NotifyWithError("Nenhum usuário encontrado com este identificador");

            return user;
        }

        private async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
                DomainNotificationHandler.NotifyWithError("Credencias inválidas");

            return signInResult.Succeeded;
        }

        private string GenerateToken(User user)
        {
            var claimsIdentity = GetClaims(user);

            var signInDate = DateTime.Now;
            var expireDate = signInDate +
                             TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtSecurityTokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = claimsIdentity,
                NotBefore = signInDate,
                Expires = expireDate
            });

            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            return token;
        }

        private ClaimsIdentity GetClaims(User user)
        {
            var claimsIdentity = new ClaimsIdentity(
                new GenericIdentity(user.Username, "SignIn"),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim("Identity", user.Id.ToString())
                }
            );

            return claimsIdentity;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUpUserAsync(SignUpUser model)
        {
            await BusConfiguration
                .BusEndpointInstance
                .SendLocal<ValidateUserCommand>(message =>
                {
                    message.Username = model.Username;
                    message.Password = model.Password;
                })
                .ConfigureAwait(false);

            //var existingUser = await _userManager.FindByNameAsync(model.Username);
            //if (existingUser != null)
            //{
            //    DomainNotificationHandler.NotifyWithError("Já existe um usuário com este identificador");
            //    return OkResponse();
            //}

            //var applicationUser = new User(model.Username);

            //var createResult = _userManager
            //    .CreateAsync(applicationUser, model.Password).Result;

            //if (!createResult.Succeeded)
            //{
            //    foreach (var createError in createResult.Errors)
            //        DomainNotificationHandler.NotifyWithError(createError.Description);
            //}

           
            

            //await _userRepository.Commit();
            
            return OkResponse();
        }
    }
}