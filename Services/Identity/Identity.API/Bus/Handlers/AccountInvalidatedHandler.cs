using System.Threading;
using System.Threading.Tasks;
using Bus.Events;
using Identity.API.Data.Repositories;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using NServiceBus;

namespace Identity.API.Bus.Handlers
{
    public class AccountInvalidatedHandler : IHandleMessages<AccountInvalidatedEvent>
    {
        private readonly UserManager<User> _userManager;

        public AccountInvalidatedHandler(
            UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(AccountInvalidatedEvent message, IMessageHandlerContext context)
        {
            var userId = message.UserId.ToString();
            var incorrectUserAdded = await _userManager.FindByIdAsync(userId);

            var delete = await _userManager.DeleteAsync(incorrectUserAdded);

            if (delete.Succeeded)
            {
                // Notify client
                return;
            }

            // Message rollback
        }
    }
}