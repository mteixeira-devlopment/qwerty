using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Identity.API.Configurations;
using Identity.API.Data.Repositories;
using Identity.API.Entities;
using Identity.API.Handlers;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services
{
    public class AuthenticationService : NotificationService, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(
            INotificationHandler notificationHandler,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            TokenConfigurations tokenConfigurations, 
            SigningConfigurations signingConfigurations,
            IUserRepository userRepository) : base(notificationHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
            _userRepository = userRepository;
        }

        public async Task SignUpUserAsync(NewUser newUser)
        {
            var existingUser = await _userManager.FindByNameAsync(newUser.Username);
            if (existingUser != null)
            {
                NotifyWithError("Já existe um usuário com este identificador");
                return;
            }

            var applicationUser = new ApplicationUser(newUser.Username);

            var createResult = _userManager
                .CreateAsync(applicationUser, newUser.Password).Result;

            if (!createResult.Succeeded)
            {
                foreach (var createError in createResult.Errors)
                    NotifyWithError(createError.Code, createError.Description);
            }

            await _userRepository.Commit();
        }

        public async Task<string> SignInUserAsync(SignInUser signInUser)
        {
            var user = await CheckIfExistsAsync(signInUser.UserIdentity);
            if (user == null)
                return string.Empty;

            var passwordIsChecked = await CheckPasswordAsync(user, signInUser.Password);

            return !passwordIsChecked ? string.Empty : GenerateToken(signInUser);
        }

        private async Task<ApplicationUser> CheckIfExistsAsync(string userIdentity)
        {
            var user = await _userManager.FindByNameAsync(userIdentity);

            if (user == null)
                NotifyWithError("Nenhum usuário encontrado com este identificador");

            return user;
        }

        private async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
                NotifyWithError("Credencias inválidas");

            return signInResult.Succeeded;
        }

        private string GenerateToken(SignInUser signInUser)
        {
            var claimsIdentity = GetClaims(signInUser);

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

        private ClaimsIdentity GetClaims(SignInUser signInUser)
        {
            var claimsIdentity = new ClaimsIdentity(
                new GenericIdentity(signInUser.UserIdentity, "SignIn"),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, signInUser.UserIdentity)
                }
            );

            return claimsIdentity;
        }
    }
}
