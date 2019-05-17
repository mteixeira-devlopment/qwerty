using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.Commands.Models;
using Deposit.API.Application.Commands.Validations;
using Deposit.API.Domain;
using MediatR;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;

using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Application.Commands.Handlers
{
    public class DepositCreditCardCommandHandler : CommandHandler<DepositCreditCardCommandModel>
    {
        private readonly IPayRepository _payRepository;
        private readonly IMediator _mediator;

        public DepositCreditCardCommandHandler(
            INotificationHandler notificationHandler, 
            IPayRepository payRepository, 
            IMediator mediator)
            : base(notificationHandler)
        {
            _payRepository = payRepository;
            _mediator = mediator;
        }

        public override async Task<CommandResponse> HandleCommand(DepositCreditCardCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<DepositCreditCardCommandValidator>(request);
            if (!validModel) return ReplyFailure();

            var providerCharge = await _payRepository.CreateCharge(request.Value);

            var chargeCommandModel = new CreateChargeCommandModel(providerCharge.ChargeId, providerCharge.Total, providerCharge.CreatedAt);
            var chargeCommandResponse = await _mediator.Send(chargeCommandModel, cancellationToken);

            if (!chargeCommandResponse.Success) return ReplyFailure();

            await _payRepository.PayCreditCard(chargeCommandModel.ChargeId, request.PaymentToken);

            return ReplySuccessful();
        }
    }
}
