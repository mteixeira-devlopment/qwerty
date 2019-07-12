using Deposit.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSeed.Data.EFConfiguration;

namespace Deposit.API.Infrastructure.Data.EFConfiguration
{
    internal sealed class ChargeConfiguration : Configuration<Charge>
    {
        public override void MapConfigure(EntityTypeBuilder<Charge> chargeConfiguration)
        {
            chargeConfiguration.Property(cha => cha.ProviderChargeId)
                .HasColumnName("id_provider_charge")
                .HasColumnType("INT")
                .IsRequired();

            chargeConfiguration.Property(cha => cha.Value)
                .HasColumnName("value")
                .HasColumnType("DECIMAL(19,2)")
                .IsRequired();

            chargeConfiguration.Property(cha => cha.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("DATETIME")
                .IsRequired();

            chargeConfiguration.Property(cha => cha.Status)
                .HasColumnName("status")
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}