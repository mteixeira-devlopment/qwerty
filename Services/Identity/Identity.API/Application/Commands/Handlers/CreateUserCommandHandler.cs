using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.Commands.Models;
using Identity.API.Application.Commands.Validations;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;
using SharedKernel.Validations;

namespace Identity.API.Application.Commands.Handlers
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommandModel>
    {
        private readonly IEventBus _eventBus;
        
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(
            IEventBus eventBus, 
            INotificationHandler notificationHandler, 
            UserManager<User> userManager, 
            IUserRepository userRepository) : base(notificationHandler)
        {
            _eventBus = eventBus;
            
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public override async Task<CommandResponse> HandleCommand(CreateUserCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<CreateUserCommandValidator>(request);
            if (!validModel) return ReplyFailure();

            var user = new User(request.Username);

            var doesNotExistUser = await CheckIfUserDoesNotExists(user);
            if (!doesNotExistUser) return ReplyFailure();

            var userCreatedSuccesfully = await TryCreateUser(user, request);
            if (!userCreatedSuccesfully) return ReplyFailure();

            await _userRepository
                .Commit()
                .ConfigureAwait(true);

            await PublishUserValidatedIntegrationEvent(user, request);

            return ReplySuccessful($"Obrigado por se cadastrar! Aguarde enquanto analisamos sua documentação.");
        }

        private async Task<bool> CheckIfUserDoesNotExists(User user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.Username).ConfigureAwait(false);
            if (existingUser == null)
                return await Task.FromResult(true);

            NotificationHandler.Notify("Este email já está em uso");

            return false;
        }

        private async Task<bool> TryCreateUser(User user, CreateUserCommandModel requestModel)
        {
            var createResult = await _userManager
                .CreateAsync(user, requestModel.Password)
                .ConfigureAwait(false);

            if (createResult.Succeeded) return await Task.FromResult(true);

            foreach (var createError in createResult.Errors)
                NotificationHandler.Notify(createError.Description);

            return false;
        }

        private async Task PublishUserValidatedIntegrationEvent(User user, CreateUserCommandModel requestModel)
        {
            var userValidated = new UserValidatedIntegrationEvent(
                user.Id, requestModel.FullName, requestModel.BirthDate, requestModel.Document);

            await Task.Run(() => _eventBus.Publish(userValidated));
        }
    }
}
