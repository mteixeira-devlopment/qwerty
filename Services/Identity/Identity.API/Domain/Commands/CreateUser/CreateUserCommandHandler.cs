using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.Events;
using Microsoft.AspNetCore.Identity;
using ServiceSeed.Commands;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;

namespace Identity.API.Domain.Commands.CreateUser
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommandModel>
    {
        private readonly IEventBus _eventBus;
        
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(
            INotificationHandler notificationHandler,
            IEventBus eventBus,              
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
            if (!validModel) return ReplyFlowFailure();

            var user = new User(request.Username);

            var doesNotExistUser = await CheckIfUserDoesNotExists(user);
            if (!doesNotExistUser) return ReplyFlowFailure();

            var userCreatedSuccesfully = await TryCreateUser(user, request);
            if (!userCreatedSuccesfully) return ReplyFlowFailure();

            await _userRepository
                .Commit()
                .ConfigureAwait(true);

            await PublishUserValidatedIntegrationEvent(user.Id, request);

            return ReplySuccessful($"Obrigado por se cadastrar! Aguarde enquanto analisamos sua documentação.");
        }

        private async Task<bool> CheckIfUserDoesNotExists(User user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.Username).ConfigureAwait(false);
            if (existingUser == null)
                return await Task.FromResult(true);

            NotificationHandler.NotifyFail("Este email já está em uso");

            return false;
        }

        private async Task<bool> TryCreateUser(User user, CreateUserCommandModel requestModel)
        {
            var createResult = await _userManager
                .CreateAsync(user, requestModel.Password)
                .ConfigureAwait(false);

            if (createResult.Succeeded) return await Task.FromResult(true);

            foreach (var createError in createResult.Errors)
                NotificationHandler.NotifyFail(createError.Description);

            return false;
        }

        private async Task PublishUserValidatedIntegrationEvent(Guid userId, CreateUserCommandModel requestModel)
        {
            var userValidated = new UserValidatedIntegrationEvent(
                userId, requestModel.FullName, requestModel.BirthDate, requestModel.Document);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(userValidated));
        }
    }
}
