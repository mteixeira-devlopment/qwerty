using System.Threading.Tasks;
using Identity.API.Configurations;
using Identity.API.Data.Repositories;
using Identity.API.SharedKernel.Handlers;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domain
{
    public class SignUpService : ISignUpService
    {
        private readonly IDomainNotificationHandler _domainNotificationHandler;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public SignUpService(
            IDomainNotificationHandler domainNotificationHandler,
            UserManager<User> userManager, 
            IUserRepository userRepository)
        {
            _domainNotificationHandler = domainNotificationHandler;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task SignUp(User user, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(user.Username);
            if (existingUser != null)
            {
                _domainNotificationHandler.NotifyWithError("Já existe um usuário com este identificador");
                return;
            }

            var createResult = _userManager
                .CreateAsync(user, password).Result;

            if (!createResult.Succeeded)
            {
                foreach (var createError in createResult.Errors)
                    _domainNotificationHandler.NotifyWithError(createError.Description);
            }

            await _userRepository.Commit();
        }
    }
}