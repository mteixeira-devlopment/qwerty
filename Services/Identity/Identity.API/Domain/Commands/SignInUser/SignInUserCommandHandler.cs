using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Identity.API.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ServiceSeed.Commands;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;

namespace Identity.API.Domain.Commands.SignInUser
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
            if (!validModel) return ReplyFlowFailure();

            var user = await GetUserIfExistsAsync(request.Username);
            if (user == null)
                return ReplyFlowFailure();

            var passwordIsChecked = await CheckPasswordAsync(user, request.Password);
            if (!passwordIsChecked)
                return ReplyFlowFailure();

            var token = GenerateToken(user);

            return ReplySuccessful(token);
        }

        private async Task<User> GetUserIfExistsAsync(string userIdentity)
        {
            var user = await _userManager.FindByNameAsync(userIdentity);

            if (user == null)
                NotificationHandler.NotifyFail("Nenhum usuário encontrado.");

            return user;
        }

        private async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
                NotificationHandler.NotifyFail("Senha incorreta.");

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