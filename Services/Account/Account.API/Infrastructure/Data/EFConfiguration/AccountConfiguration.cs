using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSeed.Data.EFConfiguration;
using Acc = Account.API.Domain.Account;

namespace Account.API.Infrastructure.Data.EFConfiguration
{
    public sealed class AccountConfiguration : Configuration<Acc>
    {
        public override void MapConfigure(EntityTypeBuilder<Acc> accountConfiguration)
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

            accountConfiguration.Property(acc => acc.Balance)
                .HasColumnName("balance")
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

        }
    }
}