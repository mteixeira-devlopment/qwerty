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

            builder
                .Property(p => p.PasswordHash)
                .HasColumnName("password-hash")
                .HasColumnType("VARCHAR(255)")
                .IsRequired();

            builder
                .Property(p => p.SecurityStamp)
                .HasColumnName("security-stamp")
                .HasColumnType("VARCHAR(255)")
                .IsRequired();
        }
    }
}