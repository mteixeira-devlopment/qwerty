﻿using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSeed.Data.EFConfiguration;

namespace Identity.API.Infrastructure.Data.EFConfiguration
{
    internal sealed class UserConfiguration : Configuration<User>
    {
        public override void MapConfigure(EntityTypeBuilder<User> builder)
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