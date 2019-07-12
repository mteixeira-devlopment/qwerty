using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using ServiceSeed.Commands;
using ServiceSeed.Responses;
using ServiceSeed.Validations;

namespace Deposit.API.Domain.Commands.CancelDepositCharge
{
    public class CancelDepositChargeCommandHandler : CommandHandler<CancelDepositChargeCommandModel>
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IEventBus _eventBus;

        public CancelDepositChargeCommandHandler(
            IDepositRepository depositRepository, 
            IEventBus eventBus)
        {
            _depositRepository = depositRepository;
            _eventBus = eventBus;
        }

        public override async Task<CommandResponse> HandleCommand(
            CancelDepositChargeCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid(request);
            if (!validModel) return ReplyFailure();

            var charge = await GetCharge(request.DepositId);
            
            charge.Cancel();

            await _depositRepository.UpdateChargeStatus(charge);

            var commited = await _depositRepository.Commit();
            if (!commited) throw new Exception($"Não foi possível cancelar a transação {charge.Id}.");

            // todo: PublishDepositCanceledIntegrationEvent();

            return ReplySuccessful();
        }

        private async Task<bool> CheckIfModelIsValid(CancelDepositChargeCommandModel requestModel)
        {
            var validator = new CommandValidator<CancelDepositChargeCommandModel, CancelDepositChargeCommandValidator>(requestModel);

            return validator.IsValid 
                ? await Task.FromResult(true)
                : throw new Exception(validator.Errors.First());
        }

        private async Task<Charge> GetCharge(Guid depositId)
        {
            var deposit = await _depositRepository.Get(depositId);
            if (deposit == null) throw new Exception($"Depósito {depositId} não encontrado para cancelamento.");

            var charge = deposit.Charge;

            return charge;
        }

        private Task PublishDepositCanceledIntegrationEvent(Guid userId, ICollection<string> cancelationReasons, CancellationToken cancellationToken)
        {
            var depositCanceledIntegrationEvent = new DepositCanceledIntegrationEvent(userId, cancelationReasons);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(depositCanceledIntegrationEvent), cancellationToken);

            return Task.CompletedTask;
        }


    }
}