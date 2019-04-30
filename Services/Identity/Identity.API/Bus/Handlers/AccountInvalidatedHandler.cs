using System.Threading.Tasks;
using Bus.Events;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Bus.Handlers
{
    //public class AccountInvalidatedHandler : IHandleMessages<AccountInvalidatedEvent>
    //{
    //    private readonly UserManager<User> _userManager;
    //    private readonly IUserRepository _userRepository;

    //    public AccountInvalidatedHandler(
    //        UserManager<User> userManager,
    //        IUserRepository userRepository)
    //    {
    //        _userManager = userManager;
    //        _userRepository = userRepository;
    //    }

    //    public async Task Handle(AccountInvalidatedEvent message, IMessageHandlerContext context)
    //    {
    //        var userId = message.UserId.ToString();
    //        var incorrectUserAdded = await _userManager.FindByIdAsync(userId);

    //        var delete = await _userManager.DeleteAsync(incorrectUserAdded);

    //        if (delete.Succeeded)
    //        {
    //            await _userRepository.Commit();
    //            // Notify client
    //            return;
    //        }

    //        // Message rollback
    //    }
    //}
}