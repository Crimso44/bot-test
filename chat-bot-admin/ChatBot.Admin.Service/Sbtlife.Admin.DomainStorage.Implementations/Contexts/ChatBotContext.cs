using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.DomainStorage.Const;
using ChatBot.Admin.DomainStorage.Contexts.Entities;
using ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot;
using ChatBot.Admin.DomainStorage.Extensions;

namespace ChatBot.Admin.DomainStorage.Contexts
{
    public class ChatBotContext : DbContext
    {
        public ChatBotContext(DbContextOptions<ChatBotContext> options)
            : base(options)
        {
        }


        public DbSet<BirthdayException> BirthdayExceptions { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<History> Historys { get; set; }
        public DbSet<Partition> Partitions { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<PatternWordRel> PatternWordRels { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordForm> WordForms { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Learning> Learnings { get; set; }
        public DbSet<ModelLearning> ModelLearnings { get; set; }
        public DbSet<ModelLearningReport> ModelLearningReports { get; set; }
        public DbSet<ModelLearningConf> ModelLearningConfs { get; set; }
        public DbSet<ModelLearningMarkup> ModelLearningMarkups { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            var guidValueEntity = builder.Entity<GuidValue>();
            guidValueEntity.HasKey(e => new { e.TokenVal });
            guidValueEntity.Property(e => e.TokenVal).ValueGeneratedNever();

            builder.ConfugueEntity<BirthdayException>(DbSchemeConst.Dto, e => e.EmployeeId);
            builder.ConfugueDtoIntEntity<Category>();
            builder.ConfugueDtoIntEntity<History>();
            builder.ConfugueEntity<Partition>(DbSchemeConst.Dto, e => e.Id);
            builder.ConfugueDtoIntEntity<Pattern>();
            builder.ConfugueEntity<PatternWordRel>(DbSchemeConst.Dto, e => new { e.PatternId, e.WordId });
            builder.ConfugueDtoIntEntity<Word>();
            builder.ConfugueDtoIntEntity<WordForm>();
            builder.ConfugueEntity<Config>("dbo", e => e.Name);
            builder.ConfugueDtoIntEntity<Learning>();
            builder.ConfugueDtoEntity<ModelLearning>();
            builder.ConfugueDtoIntEntity<ModelLearningReport>();
            builder.ConfugueDtoIntEntity<ModelLearningConf>();
            builder.ConfugueDtoIntEntity<ModelLearningMarkup>();
        }
    }
}
