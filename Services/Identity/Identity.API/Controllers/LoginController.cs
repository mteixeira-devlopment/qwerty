using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Identity.API.Enumerations;
using Identity.API.Handlers;
using Identity.API.Models;
using Identity.API.Services;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiController
    {
        private readonly INotificationHandler _notificationHandler;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(
            INotificationHandler notificationHandler,
            IAuthenticationService authenticationService) : base(notificationHandler)
        {
            _notificationHandler = notificationHandler;
            _authenticationService = authenticationService;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public object Post([FromBody]User usuario)
        //{
        //    bool credenciaisValidas = false;
        //    if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserID))
        //    {
        //        // Verifica a existência do usuário nas tabelas do
        //        // ASP.NET Core Identity
        //        var userIdentity = userManager
        //            .FindByNameAsync(usuario.UserID).Result;
        //        if (userIdentity != null)
        //        {
        //            // Efetua o login com base no Id do usuário e sua senha
        //            var resultadoLogin = signInManager
        //                .CheckPasswordSignInAsync(userIdentity, usuario.Password, false)
        //                .Result;
        //            if (resultadoLogin.Succeeded)
        //            {
        //                // Verifica se o usuário em questão possui
        //                // a role Acesso-APIAlturas
        //                credenciaisValidas = userManager.IsInRoleAsync(
        //                    userIdentity, Role.ProjectOwner.Name).Result;
        //            }
        //        }
        //    }

        //    if (credenciaisValidas)
        //    {
        //        ClaimsIdentity identity = new ClaimsIdentity(
        //            new GenericIdentity(usuario.UserID, "Login"),
        //            new[] {
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        //                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserID)
        //            }
        //        );

        //        DateTime dataCriacao = DateTime.Now;
        //        DateTime dataExpiracao = dataCriacao +
        //                                 TimeSpan.FromSeconds(tokenConfigurations.Seconds);

        //        var handler = new JwtSecurityTokenHandler();
        //        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        //        {
        //            Issuer = tokenConfigurations.Issuer,
        //            Audience = tokenConfigurations.Audience,
        //            SigningCredentials = signingConfigurations.SigningCredentials,
        //            Subject = identity,
        //            NotBefore = dataCriacao,
        //            Expires = dataExpiracao
        //        });
        //        var token = handler.WriteToken(securityToken);

        //        return new
        //        {
        //            authenticated = true,
        //            created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
        //            expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
        //            accessToken = token,
        //            message = "OK"
        //        };
        //    }
        //    else
        //    {
        //        return new
        //        {
        //            authenticated = false,
        //            message = "Falha ao autenticar"
        //        };
        //    }
        //}

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateDefaultUserAsync(NewUser model)
        {
            await _authenticationService.CreateUser(model);
            return OkResponse(new {A = "Ana", B = "Pedro"});
        }
    }
}