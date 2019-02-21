using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.API.Entities;

namespace Organization.API.Data.EFConfiguration
{
    internal class OrganizationConfiguration : Configuration<Org>
    {
        public override void MapConfiguration(EntityTypeBuilder<Org> builder)
        {
            builder
                .Property(p => p.Name)
                .HasColumnName("name")
                .HasColumnType("VARCHAR(80)")
                .IsRequired();
        }
    }
}