using Deposit.API.Infrastructure.Data.EFConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Deposit.API.Infrastructure.Data
{
    public sealed class DepositContext : DbContext
    {
        public DepositContext(DbContextOptions<DepositContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepositConfiguration());
            modelBuilder.ApplyConfiguration(new ChargeConfiguration());
        }
    }
}
