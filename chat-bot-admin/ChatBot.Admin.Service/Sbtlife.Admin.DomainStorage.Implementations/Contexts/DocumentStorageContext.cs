using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.DomainStorage.Const;
using ChatBot.Admin.DomainStorage.Contexts.Entities;
using ChatBot.Admin.DomainStorage.Contexts.Entities.DocumentStorage;
using ChatBot.Admin.DomainStorage.Extensions;

namespace ChatBot.Admin.DomainStorage.Contexts
{
    public class DocumentStorageContext : DbContext
    {
        public DocumentStorageContext(DbContextOptions<DocumentStorageContext> options)
            : base(options)
        {
        }


        public DbSet<Files> Files { get; set; }
        public DbSet<FilesCatalog> FilesCatalog { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var guidValueEntity = builder.Entity<GuidValue>();
            guidValueEntity.HasKey(e => new { e.TokenVal });
            guidValueEntity.Property(e => e.TokenVal).ValueGeneratedNever();

            builder.Entity<Files>().HasKey(x => x.Id);
            builder.Entity<FilesCatalog>().HasKey(x => x.Id);

        }
    }
}
