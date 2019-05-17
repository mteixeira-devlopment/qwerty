using System.Threading.Tasks;
using Deposit.API.Domain;

namespace Deposit.API.Infrastructure.Data.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly DepositContext _depositContext;

        public DepositRepository(DepositContext depositContext)
        {
            _depositContext = depositContext;
        }

        public async Task<Charge> CreateCharge(Charge charge)
        {
            await _depositContext.Set<Charge>().AddAsync(charge);
            return charge;
        }
    }
}