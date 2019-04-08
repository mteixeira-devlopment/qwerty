using Account.API.Domain.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.API.Infrastructure.Data.EFConfiguration
{
    public abstract class Configuration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> configuration)
        {
            var entityUpperName = typeof(TEntity).Name.ToUpperInvariant();
            configuration.ToTable(entityUpperName);

            configuration
                .HasKey(e => e.Id);

            configuration.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("VARCHAR(38)")
                .IsRequired();

            MapConfigure(configuration);
        }

        public abstract void MapConfigure(EntityTypeBuilder<TEntity> builder);
    }
}