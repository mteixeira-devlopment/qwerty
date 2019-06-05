using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.Commands.Models;
using Deposit.API.Application.Commands.Validations;
using Deposit.API.Domain;
using Deposit.API.Domain.DataTransferObjects;
using Deposit.API.Infrastructure.Data.ExternalRepositories;
using MediatR;
using Newtonsoft.Json;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;

using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Application.Commands.Handlers
{
    public class DepositCreditCardCommandHandler : CommandHandler<DepositCreditCardCommandModel>
    {
        private readonly IPayExternalRepository _payRepository;
        private readonly IMediator _mediator;

        public DepositCreditCardCommandHandler(
            INotificationHandler notificationHandler, 
            IPayExternalRepository payRepository, 
            IMediator mediator) : base(notificationHandler)
        {
            _payRepository = payRepository;
            _mediator = mediator;
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

            var paid = await PayCharge(request.PaymentToken, providerCharge.ChargeId);
            if (!paid) ReplyFailure();

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
    }
}
