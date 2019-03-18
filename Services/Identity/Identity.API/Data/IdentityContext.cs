using Identity.API.Data.EFConfiguration;
using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
