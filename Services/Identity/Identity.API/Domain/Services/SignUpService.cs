using System.Threading.Tasks;
using Identity.API.Domain.Handlers;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domain.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly INotificationHandler _domainNotificationHandler;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public SignUpService(
            INotificationHandler domainNotificationHandler,
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
                _domainNotificationHandler.Notify("Este usuário já está em uso");
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