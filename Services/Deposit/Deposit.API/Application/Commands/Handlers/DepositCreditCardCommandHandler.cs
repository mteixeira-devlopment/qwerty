using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.Commands.Models;
using Deposit.API.Application.Commands.Validations;
using Deposit.API.Domain;
using SharedKernel.Commands;
using SharedKernel.Handlers;
using SharedKernel.Responses;

using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Application.Commands.Handlers
{
    public class DepositCreditCardCommandHandler : CommandHandler<DepositCreditCardCommandModel>
    {
        private readonly IPayRepository _payRepository;

        public DepositCreditCardCommandHandler(
            INotificationHandler notificationHandler, 
            IPayRepository payRepository)
            : base(notificationHandler)
        {
            _payRepository = payRepository;
        }

        public override async Task<CommandResponse> HandleCommand(DepositCreditCardCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<DepositCreditCardCommandValidator>(request);
            if (!validModel) return ReplyFailure();

            var paymentMethod = new CreditCard(
                request.AccountId, 
                request.CreditCardName, 
                request.CreditCardNumber, 
                request.CreditCardExpirationYear, 
                request.CreditCardExpirationMonth, 
                request.CreditCardSecurityNumber);

            var deposit = new Depos(request.Value, paymentMethod);

            await _payRepository.PayCreditCard(deposit, request.PaymentToken, request.CreditCardMask);

            return ReplySuccessful();
        }
    }
}
