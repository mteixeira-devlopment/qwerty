using System.Linq;
using System.Threading.Tasks;
using Identity.API.Handlers;
using Identity.API.Models;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Services
{
    public class AuthenticationService : NotificationService, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(
            INotificationHandler notificationHandler,
            [FromServices] UserManager<ApplicationUser> userManager) : base(notificationHandler)
        {
            _userManager = userManager;
        }

        public async Task CreateUser(NewUser newUser)
        {
            var existingUser = await _userManager.FindByNameAsync(newUser.Username);
            if (existingUser != null)
            {
                NotifyWithError("Já existe um usuário com este usuário");
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
    }
}
