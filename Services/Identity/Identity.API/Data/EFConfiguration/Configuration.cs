using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Data.EFConfiguration
{
    internal abstract class Configuration<TEntity> : IEntityTypeConfiguration<TEntity>
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