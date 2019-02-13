using System.Threading.Tasks;
using Identity.API.Configurations;
using Identity.API.Handlers;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public class AuthenticationService : NotificationService, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public AuthenticationService(
            INotificationHandler notificationHandler,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
            : base(notificationHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        public void CreateUser()
        {
            throw new System.NotImplementedException();
        }

        //public async Task CreateUser()
        //{
        //    if (await _userManager.FindByNameAsync(user.UserName) != null) return;

        //    var resultado = _userManager
        //        .CreateAsync(user, password).Result;

        //    if (resultado.Succeeded &&
        //        !string.IsNullOrWhiteSpace(initialRole))
        //    {
        //        _userManager.AddToRoleAsync(user, initialRole).Wait();
        //    }
        //}
    }
}
