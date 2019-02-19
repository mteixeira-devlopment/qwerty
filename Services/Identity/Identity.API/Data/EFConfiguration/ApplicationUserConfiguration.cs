using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Data.EFConfiguration
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

            builder
                .Property(p => p.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("VARCHAR(255)")
                .IsRequired();

            builder
                .Property(p => p.SecurityStamp)
                .HasColumnName("security_stamp")
                .HasColumnType("VARCHAR(255)")
                .IsRequired();
        }
    }
}