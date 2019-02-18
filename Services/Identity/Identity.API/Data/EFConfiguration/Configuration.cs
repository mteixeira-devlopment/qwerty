﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Data
{
    internal abstract class Configuration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TEntity>
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
                .ValueGeneratedOnAdd()
                .IsRequired();

            MapConfiguration(builder);
        }

        public abstract void MapConfiguration(EntityTypeBuilder<TEntity> builder);
    }
}