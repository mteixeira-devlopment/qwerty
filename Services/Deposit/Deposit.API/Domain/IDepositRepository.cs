using System;
using System.Threading.Tasks;

namespace Deposit.API.Domain
{
    public interface IDepositRepository
    {
        Task<Deposit> CreateAsync(Deposit deposit);
        Task<Deposit> Get(Guid id);
        Task UpdateChargeStatus(Charge charge);
        Task<bool> Commit();
    }
}