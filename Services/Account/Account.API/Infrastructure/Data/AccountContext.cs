using Account.API.Infrastructure.Data.EFConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Account.API.Infrastructure.Data
{
    public sealed class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
