using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSeed.Data.EFConfiguration;
using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Infrastructure.Data.EFConfiguration
{
    internal sealed class DepositConfiguration : Configuration<Depos>
    {
        public override void MapConfigure(EntityTypeBuilder<Depos> depositConfiguration)
        {
            depositConfiguration.Property<Guid>("ChargeId")
                .HasColumnName("id_charge")
                .HasColumnType("VARCHAR(38)")
                .HasField("_chargeId")
                .IsRequired();

            depositConfiguration
                .HasOne(dep => dep.Charge)
                .WithMany()
                .HasForeignKey("ChargeId");

            depositConfiguration.Property(dep => dep.AccountId)
                .HasColumnName("id_account")
                .HasColumnType("VARCHAR(38)")
                .IsRequired();

        }
    }
}