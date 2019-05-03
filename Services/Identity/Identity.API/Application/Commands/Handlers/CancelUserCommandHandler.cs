using System.Threading;
using System.Threading.Tasks;
using Identity.API.Application.Commands.Models;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Application.Commands.Handlers
{
    public class CancelUserCommandHandler : IRequestHandler<CancelUserCommandModel, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public CancelUserCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            
        }

        public async Task<bool> Handle(CancelUserCommandModel request, CancellationToken cancellationToken)
        {
            var userId = request.UserId.ToString();
            var incorrectUserAdded = await _userManager.FindByIdAsync(userId);

            var delete = await _userManager.DeleteAsync(incorrectUserAdded);

            if (!delete.Succeeded)
                return false;

            await _userRepository.Commit();
            return true;
        }
    }
}