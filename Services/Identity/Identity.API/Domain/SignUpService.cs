using System.Threading.Tasks;
using Identity.API.Configurations;
using Identity.API.Data.Repositories;
using Identity.API.SharedKernel.Handlers;
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

        public async Task<bool> SignUp(User user, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(user.Username);
            if (existingUser != null)
            {
                _domainNotificationHandler.Notify("Já existe um usuário com este identificador");
                return false;
            }

            var createResult = _userManager
                .CreateAsync(user, password).Result;

            if (!createResult.Succeeded)
            {
                foreach (var createError in createResult.Errors)
                    _domainNotificationHandler.Notify(createError.Description);

                return false;
            }

            await _userRepository.Commit();
            return true;
        }
    }
}