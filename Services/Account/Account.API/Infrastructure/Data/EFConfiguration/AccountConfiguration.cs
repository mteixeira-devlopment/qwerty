using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.API.Infrastructure.Data.EFConfiguration
{
    public sealed class AccountConfiguration : Configuration<Domain.Account>
    {
        public override void MapConfigure(EntityTypeBuilder<Domain.Account> accountConfiguration)
        {
            accountConfiguration.Property(acc => acc.UserId)
                .HasColumnName("id_user")
                .HasColumnType("VARCHAR(38)")
                .IsRequired();

            accountConfiguration.Property<Guid>("CustomerId")
                .HasColumnName("id_customer")
                .HasColumnType("VARCHAR(38)")
                .HasField("_customerId")
                .IsRequired();

            accountConfiguration.HasOne(acc => acc.Customer)
                .WithMany()
                .HasForeignKey("CustomerId");
        }
    }
}