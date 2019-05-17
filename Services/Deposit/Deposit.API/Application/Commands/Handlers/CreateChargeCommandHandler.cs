using System.Threading;
using System.Threading.Tasks;
using Deposit.API.Application.Commands.Models;
using Deposit.API.Application.Commands.Validations;
using Deposit.API.Domain;
using SharedKernel.Commands;
using SharedKernel.Responses;

namespace Deposit.API.Application.Commands.Handlers
{
    public class CreateChargeCommandHandler : CommandHandler<CreateChargeCommandModel>
    {
        private readonly IDepositRepository _depositRepository;

        public CreateChargeCommandHandler(IDepositRepository depositRepository)
        {
            _depositRepository = depositRepository;
        }

        public override async Task<CommandResponse> HandleCommand(CreateChargeCommandModel request, CancellationToken cancellationToken)
        {
            var validModel = await CheckIfModelIsValid<CreateChargeCommandValidator>(request);
            if (!validModel) return ReplyFailure();

            var charge = new Charge(request.ChargeId, request.Value, request.CreatedAt);
            await _depositRepository.CreateCharge(charge);

            return ReplySuccessful();
        }
    }
}