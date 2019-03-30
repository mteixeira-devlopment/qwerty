using Account.API.Domain;
using Account.API.Domain.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Acc = Account.API.Domain.Account;

namespace Account.API.Data.EFConfiguration
{
    public class AccountConfiguration : Configuration<Acc>
    {
        public override void MapConfiguration(EntityTypeBuilder<Acc> builder)
        {
            builder.Property<Guid>("CustomerId")
                .HasColumnName("id_customer")
                .HasColumnType("VARCHAR(38)")
                .HasField("_customerId")
                .IsRequired();

            builder.HasOne(acc => acc.Customer)
                .WithMany()
                .HasForeignKey("CustomerId");
        }
    }

    public abstract class Configuration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        private EntityTypeBuilder<TEntity> _builder;

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            _builder = builder;

            _builder
                .HasKey(e => e.Id);

            _builder
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("VARCHAR(38)")
                .IsRequired();

            MapConfiguration(builder);
        }

        public abstract void MapConfiguration(EntityTypeBuilder<TEntity> builder);
    }
}