using System;
using System.Collections.Generic;
using MediatR;
using ServiceSeed.Responses;

namespace Deposit.API.Domain.Commands.CancelDepositCharge
{
    public class CancelDepositChargeCommandModel : IRequest<CommandResponse>
    {
        public Guid DepositId { get; private set; }
        public ICollection<string> CancellationReasons { get; private set; }

        public CancelDepositChargeCommandModel(Guid depositId, ICollection<string> cancellationReasons)
        {
            DepositId = depositId;
            CancellationReasons = cancellationReasons;
        }
    }
}
