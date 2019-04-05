using Microsoft.EntityFrameworkCore;
using Notification.API.Infrastructure.Data.EFConfiguration;

namespace Notification.API.Infrastructure.Data
{
    public sealed class NotificationContext : DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationUserConfiguration());
        }
    }
}
