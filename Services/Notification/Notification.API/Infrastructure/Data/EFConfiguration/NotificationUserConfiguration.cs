using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.API.Domain;
using SharedKernel.Data.EFConfiguration;

namespace Notification.API.Infrastructure.Data.EFConfiguration
{
    public class NotificationUserConfiguration : Configuration<NotificationUser>
    {
        public override void MapConfigure(EntityTypeBuilder<NotificationUser> notificationUserConfiguration)
        {
            notificationUserConfiguration.Property(acc => acc.UserId)
                .HasColumnName("id_user")
                .HasColumnType("VARCHAR(38)")
                .IsRequired();

            notificationUserConfiguration.Property<Guid>("NotificationId")
                .HasColumnName("id_notification")
                .HasColumnType("VARCHAR(38)")
                .HasField("_notificationId")
                .IsRequired();

            notificationUserConfiguration.HasOne(acc => acc.Notification)
                .WithMany()
                .HasForeignKey("NotificationId");

            notificationUserConfiguration.Property(not => not.SentIn)
                .HasColumnName("sent_in")
                .HasColumnType("DATETIME")
                .IsRequired();
        }
    }
}