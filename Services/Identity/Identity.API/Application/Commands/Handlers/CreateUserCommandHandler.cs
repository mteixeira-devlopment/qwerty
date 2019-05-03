using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.Commands.Models;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.API.Domain;
using Identity.API.Domain.Handlers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Application.Commands.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandModel, bool>
    {
        private readonly IEventBus _eventBus;
        private readonly IMediator _metiator;

        private readonly INotificationHandler _domainNotificationHandler;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(
            IEventBus eventBus, 
            IMediator metiator, 
            INotificationHandler domainNotificationHandler, 
            UserManager<User> userManager, 
            IUserRepository userRepository)
        {
            _eventBus = eventBus;
            _metiator = metiator;

            _domainNotificationHandler = domainNotificationHandler;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateUserCommandModel request, CancellationToken cancellationToken)
        {
            var user = new User(request.Username);

            var existingUser = await _userManager.FindByNameAsync(user.Username).ConfigureAwait(false);
            if (existingUser != null)
            {
                _domainNotificationHandler.Notify("Este email já está em uso");

                return false;
            }

            var createResult = await _userManager
                .CreateAsync(user, request.Password).ConfigureAwait(false);

            if (!createResult.Succeeded)
            {
                foreach (var createError in createResult.Errors)
                    _domainNotificationHandler.Notify(createError.Description);

                return false;
            }

            await _userRepository.Commit().ConfigureAwait(false);

            var userValidated = new UserValidatedIntegrationEvent(
                user.Id, request.FullName, request.BirthDate, request.Document);

            _eventBus.Publish(userValidated);

            return true;
        }
    }
}
