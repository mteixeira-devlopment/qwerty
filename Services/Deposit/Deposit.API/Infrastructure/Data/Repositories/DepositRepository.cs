using System;
using System.Threading.Tasks;
using Deposit.API.Domain;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Depos> Get(Guid id)
        {
            return await _depositContext.Set<Depos>()
                .Include(d => d.Charge)
                .FirstOrDefaultAsync(dep => dep.Id == id);
        }

        public async Task UpdateChargeStatus(Charge charge)
        {
            await Task.FromResult(_depositContext
                .Entry(charge)
                .Property(ch => ch.Status)
                .IsModified = true);
        }

        public async Task<bool> Commit()
            => await _depositContext.SaveChangesAsync() > 0;
    }
}