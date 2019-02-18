using Identity.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Data
{
    internal sealed class ApplicationUserConfiguration : Configuration<ApplicationUser>
    {
        public override void MapConfiguration(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .Property(p => p.Username)
                .HasColumnName("username")
                .HasColumnType("VARCHAR(80)")
                .IsRequired();
        }
    }
}