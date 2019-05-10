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
            
        }
    }
}
