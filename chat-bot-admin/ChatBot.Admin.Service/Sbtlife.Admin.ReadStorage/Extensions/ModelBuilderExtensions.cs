using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatBot.Admin.ReadStorage.Const;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;

namespace ChatBot.Admin.ReadStorage.Extensions
{
    static class ModelBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ConfugueViewEntity<TEntity>(this ModelBuilder builder)
            where TEntity : Entity
        {
            var entity = builder.Entity<TEntity>()
                .ToTable(GetViewTypeName<TEntity>(), AppConst.DbSchemeConst.Dbo);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();

            return entity;
        }

        public static EntityTypeBuilder<TEntity> ConfugueViewEntityInt<TEntity>(this ModelBuilder builder)
            where TEntity : EntityInt
        {
            var entity = builder.Entity<TEntity>()
                .ToTable(GetViewTypeName<TEntity>(), AppConst.DbSchemeConst.Dbo);
            entity.HasKey(e => e.Id);

            return entity;
        }

        public static EntityTypeBuilder<TEntity> ConfugueDboEntity<TEntity>(this ModelBuilder builder, Expression<Func<TEntity, object>> keyFunc)
            where TEntity : class
        {
            var entity = builder.Entity<TEntity>()
                .ToTable(GetTypeName<TEntity>(), AppConst.DbSchemeConst.Dbo);

            entity.HasKey(keyFunc);

            return entity;
        }

        private static string GetViewTypeName<TEntity>()
        {
            return $"{AppConst.ViewPrefix}{typeof(TEntity).Name}";
        }

        private static string GetTypeName<TEntity>()
        {
            return typeof(TEntity).Name;
        }
    }
}
