using Microsoft.EntityFrameworkCore;
using Organization.API.Data.EFConfiguration;

namespace Organization.API.Data
{
    public class OrganizationContext : DbContext
    {
        public OrganizationContext(DbContextOptions<OrganizationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        }
    }
}
