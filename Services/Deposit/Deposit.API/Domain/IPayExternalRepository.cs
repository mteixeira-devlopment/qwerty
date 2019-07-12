using System.Threading.Tasks;
using Deposit.API.Domain.DTOs;
using Deposit.API.Infrastructure.Data.ExternalRepositories;

namespace Deposit.API.Domain
{
    public interface IPayExternalRepository
    {
        Task<ExternalResponse<ChargeTransferObject>> CreateCharge(ChargeBodyTransferObject chargeBody);
        Task<ExternalResponse<PaymentTransferObject>> PayCreditCard(int chargeId, PaymentCreditCardBodyTransferObject paymentBody);
    }
}