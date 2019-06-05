using System.Threading.Tasks;
using Deposit.API.Domain;

using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Infrastructure.Data.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly DepositContext _depositContext;

        public DepositRepository(DepositContext depositContext)
        {
            _depositContext = depositContext;
        }

        public async Task<Depos> CreateAsync(Depos deposit)
        {
            await _depositContext.AddAsync(deposit);
            return deposit;
        }

        public async Task Commit()
        {
            await _depositContext.SaveChangesAsync();
        }
    }
}