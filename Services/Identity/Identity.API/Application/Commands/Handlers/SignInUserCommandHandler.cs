using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Identity.API.Application.Commands.Models;
using Identity.API.Application.Commands.Validations;
using Identity.API.Configurations;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;
using SharedKernel.Validations;

namespace Identity.API.Application.Commands.Handlers
{
    public class SignInUserCommandHandler : CommandHandler<SignInUserCommandModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly TokenConfigurations _tokenConfigurations;
        private readonly CredentialConfigurations _credentialConfigurations;

        public SignInUserCommandHandler(
            INotificationHandler notificationHandler, 
            UserManager<User> userManager,
            SignInManager<User> signInManager, 
            TokenConfigurations tokenConfigurations, 
            CredentialConfigurations credentialConfigurations) : base(notificationHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _tokenConfigurations = tokenConfigurations;
            _credentialConfigurations = credentialConfigurations;
        }

        public override async Task<CommandResponse> HandleCommand(SignInUserCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<SignInUserCommandValidator>(request);
            if (!validModel) return ReplyFailure();

            var user = await GetUserIfExistsAsync(request.Username);
            if (user == null)
                return ReplyFailure();

            var passwordIsChecked = await CheckPasswordAsync(user, request.Password);
            if (!passwordIsChecked)
                return ReplyFailure();

            var token = GenerateToken(user);

            return ReplySuccessful(token);
        }

        private async Task<User> GetUserIfExistsAsync(string userIdentity)
        {
            var user = await _userManager.FindByNameAsync(userIdentity);

            if (user == null)
                NotificationHandler.Notify("Nenhum usuário encontrado.");

            return user;
        }

        private async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
                NotificationHandler.Notify("Senha incorreta.");

            return signInResult.Succeeded;
        }

        private string GenerateToken(User user)
        {
            var claimsIdentity = GetClaims(user);

            var signInDate = DateTime.Now;
            var expireDate = signInDate +
                             TimeSpan.FromSeconds(_tokenConfigurations.SecondsValid);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtSecurityTokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _credentialConfigurations.SigningCredentials,
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
    }
}