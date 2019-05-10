using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Seed;

namespace Identity.API.Infrastructure.Data.EFConfiguration
{
    internal abstract class Configuration<TEntity> : IEntityTypeConfiguration<TEntity>
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

            MapConfiguration(configuration);
        }

        public abstract void MapConfiguration(EntityTypeBuilder<TEntity> builder);
    }
}