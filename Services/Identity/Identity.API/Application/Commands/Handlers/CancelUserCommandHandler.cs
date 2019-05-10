using System.Threading;
using System.Threading.Tasks;
using Identity.API.Application.Commands.Models;
using Identity.API.Application.Commands.Validations;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;
using SharedKernel.Validations;

namespace Identity.API.Application.Commands.Handlers
{
    public class CancelUserCommandHandler : CommandHandler<CancelUserCommandModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public CancelUserCommandHandler(
            INotificationHandler notificationHandler,
            UserManager<User> userManager,
            IUserRepository userRepository) : base (notificationHandler)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public override async Task<CommandResponse> HandleCommand(CancelUserCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid(request);
            if (!validModel) return ReplyFailure();

            var userId = request.UserId.ToString();
            var incorrectUserAdded = await _userManager.FindByIdAsync(userId);

            var delete = await _userManager.DeleteAsync(incorrectUserAdded);

            if (!delete.Succeeded)
                return ReplyFailure();

            await _userRepository.Commit();
            return ReplySuccessful();
        }

        private async Task<bool> CheckIfModelIsValid(CancelUserCommandModel requestModel)
        {
            var validator = new CommandValidator<CancelUserCommandModel, CancelUserCommandValidator>(requestModel);

            if (validator.IsValid) return await Task.FromResult(true);

            foreach (var error in validator.Errors)
                NotificationHandler.Notify(error);

            // TODO: IMPLEMENTAR LANÇAMENTO DO EVENTO PARA O SERVIÇO DE MONITORAMENTO INFORMANDO QUE AINDA HÁ UM USUÁRIO SUJO NA BASE

            return await Task.FromResult(false);
        }
    }
}