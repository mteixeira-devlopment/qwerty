using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Data.EFConfiguration;

namespace Notification.API.Infrastructure.Data.EFConfiguration
{
    public class NotificationConfiguration : Configuration<Domain.Notification>
    {
        public override void MapConfigure(EntityTypeBuilder<Domain.Notification> notificationConfiguration)
        {
            notificationConfiguration.Property(not => not.Title)
                .HasColumnName("title")
                .HasColumnType("VARCHAR(120)")
                .IsRequired();

            notificationConfiguration.Property(not => not.Summary)
                .HasColumnName("summary")
                .HasColumnType("VARCHAR(500)")
                .IsRequired();
        }
    }
}