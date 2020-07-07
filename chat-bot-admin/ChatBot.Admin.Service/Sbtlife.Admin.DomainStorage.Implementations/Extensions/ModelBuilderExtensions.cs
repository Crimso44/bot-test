using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatBot.Admin.DomainStorage.Const;
using ChatBot.Admin.DomainStorage.Contexts.Entities;

namespace ChatBot.Admin.DomainStorage.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ConfugueDtoEntity<TEntity>(this ModelBuilder builder)
            where TEntity : Entity
        {
            var entity = builder.Entity<TEntity>()
                .ToTable(GetTypeName<TEntity>(), DbSchemeConst.Dto);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            return entity;
        }

        public static EntityTypeBuilder<TEntity> ConfugueDtoIntEntity<TEntity>(this ModelBuilder builder)
            where TEntity : EntityInt
        {
            var entity = builder.Entity<TEntity>()
                .ToTable(GetTypeName<TEntity>(), DbSchemeConst.Dto);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            return entity;
        }

        public static EntityTypeBuilder<TEntity> ConfugueEntity<TEntity>(this ModelBuilder builder, string dbScheme, Expression<Func<TEntity, object>> keyFunc)
            where TEntity : class
        {
            var entity = builder.Entity<TEntity>()
                .ToTable(GetTypeName<TEntity>(), dbScheme);

            entity.HasKey(keyFunc);

            return entity;
        }

        private static string GetTypeName<TEntity>()
        {
            return typeof(TEntity).Name;
        }
    }
}
