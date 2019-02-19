using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Data.EFConfiguration
{
    internal class GatewayContext : DbContext
    {
        public GatewayContext(DbContextOptions<GatewayContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}