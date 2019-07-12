﻿using System.Threading.Tasks;
using EventBus.Abstractions;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.API.Domain.Commands.CancelUser;
using MediatR;

namespace Identity.API.Application.IntegrationEvents.EventHandlers
{
    public class AccountInvalidatedIntegrationEventHandler : IIntegrationEventHandler<AccountInvalidatedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public AccountInvalidatedIntegrationEventHandler(
            IMediator mediator, 
            IEventBus eventBus)
        {
            _mediator = mediator;
            _eventBus = eventBus;
        }

        public async Task Handle(AccountInvalidatedIntegrationEvent @event)
        {
            var cancelUser = new CancelUserCommandModel(@event.UserId);
            await _mediator.Send(cancelUser);
        }

        public void Handle2()
        {
            throw new System.NotImplementedException();
        }
    }
}