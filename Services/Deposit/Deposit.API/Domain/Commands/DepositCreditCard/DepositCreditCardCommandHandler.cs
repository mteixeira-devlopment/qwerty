using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.IntegrationEvents.Events;
using Deposit.API.Domain.Commands.CreateDeposit;
using Deposit.API.Domain.DTOs;
using EventBus.Abstractions;
using MediatR;
using ServiceSeed.Commands;
using ServiceSeed.Handlers;
using ServiceSeed.Responses;

namespace Deposit.API.Domain.Commands.DepositCreditCard
{
    public class DepositCreditCardCommandHandler : CommandHandler<DepositCreditCardCommandModel>
    {
        private readonly IPayExternalRepository _payRepository;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public DepositCreditCardCommandHandler(
            INotificationHandler notificationHandler, 
            IPayExternalRepository payRepository, 
            IMediator mediator, 
            IEventBus eventBus) : base(notificationHandler)
        {
            _payRepository = payRepository;
            _mediator = mediator;
            _eventBus = eventBus;
        }

        public override async Task<CommandResponse> HandleCommand(DepositCreditCardCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<DepositCreditCardCommandValidator>(request);
            if (!validModel) return ReplyFailure();

            var providerCharge = await CreateCharge(request.Value, cancellationToken);
            if (providerCharge == null) return ReplyFailure();

            var createDepositCommandModel = new CreateDepositCommandModel(
                request.AccountId, providerCharge.ChargeId, providerCharge.Total, providerCharge.CreatedAt);

            var createDepositResponse = await _mediator.Send(createDepositCommandModel, cancellationToken);
            if (!createDepositResponse.Success) return ReplyFailure();

            var depositId = (Guid) createDepositResponse.Content;

            var paid = await PayCharge(request.PaymentToken, providerCharge.ChargeId);
            if (!paid) ReplyFailure();

            await PublishDepositCreatedIntegrationEvent(depositId, request.AccountId, request.Value);

            return ReplySuccessful($"Transação iniciada com sucesso! Aguarde enquanto confirmamos o pagamento.");
        }

        private async Task<ChargeTransferObject.Data> CreateCharge(decimal chargeValue, CancellationToken cancellationToken)
        {
            var chargeBody = new ChargeBodyTransferObject(chargeValue);

            var chargingResponse = await _payRepository.CreateCharge(chargeBody);
            if (!chargingResponse.Success)
            {
                NotificationHandler.Notify(chargingResponse.Error);
                return null;
            }

            var providerCharge = chargingResponse.Content.DataObject;
            return providerCharge;
        }

        private async Task<bool> PayCharge(string paymentToken, int providerChargeId)
        {
            var paymentBody = new PaymentCreditCardBodyTransferObject(paymentToken);
            var paymentObject = paymentBody.PaymentObject;

            paymentObject.Card.AddBillingAddress("Av Darcy Vargas", 713, "Ipiranga", "36031100", "Juiz de Fora", "MG");
            paymentObject.Card.AddCustomer("Maycon Teixeira", "mteixeira.dev@outlook.com", "11709501677", DateTime.Now, "32991179841");

            var paymentResponse = await _payRepository.PayCreditCard(providerChargeId, paymentBody);

            if (!paymentResponse.Success)
            {
                NotificationHandler.Notify(paymentResponse.Error);
                return false;
            }

            return true;
        }

        private Task PublishDepositCreatedIntegrationEvent(Guid depositId, Guid accountId, decimal value)
        {
            var depositCreated = new DepositCreatedIntegrationEvent(depositId, accountId, value);

            #pragma warning disable 4014
            Task.Run(() => _eventBus.Publish(depositCreated));

            return Task.CompletedTask;
        }
    }
}
