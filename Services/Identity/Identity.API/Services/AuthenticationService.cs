using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Identity.API.Configurations;
using Identity.API.Data;
using Identity.API.Enumerations;
using Identity.API.Handlers;
using Identity.API.Models;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services
{
    public class AuthenticationService : NotificationService, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IdentityContext _context;

        public AuthenticationService(
            INotificationHandler notificationHandler,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            TokenConfigurations tokenConfigurations, 
            SigningConfigurations signingConfigurations) : base(notificationHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
        }

        public async Task CreateUser(NewUser newUser)
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
        }

        public async Task<string> SignInUser(SignInUser signInUser)
        {
            _context.Database.EnsureCreated();

            var userIdentity = await _userManager.FindByNameAsync(signInUser.UserIdentity);
            if (userIdentity == null)
            {
                NotifyWithError("Nenhum usuário encontrado com este identificador");
                return string.Empty;
            }

            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(userIdentity, signInUser.Password, false);
            if (!signInResult.Succeeded)
            {
                NotifyWithError("Credencias inválidas");
                return string.Empty;
            }

            var claimsIdentity = new ClaimsIdentity(
                new GenericIdentity(signInUser.UserIdentity, "SignIn"),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, signInUser.UserIdentity)
                }
            );

            DateTime signInDate = DateTime.Now;
            DateTime expireDate = signInDate +
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

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}
