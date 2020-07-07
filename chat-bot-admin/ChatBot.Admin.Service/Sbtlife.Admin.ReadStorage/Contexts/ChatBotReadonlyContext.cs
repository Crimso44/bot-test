using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.ReadStorage.Contexts.Abstractions;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;
using ChatBot.Admin.ReadStorage.Extensions;

namespace ChatBot.Admin.ReadStorage.Contexts
{
    class ChatBotReadonlyContext : DbContext, IChatBotReadonlyContext
    {
        public ChatBotReadonlyContext(DbContextOptions<ChatBotReadonlyContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<BirthdayException> BirthdayExceptions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Learning> Learnings { get; set; }
        public DbSet<ModelLearning> ModelLearnings { get; set; }
        public DbSet<ModelLearningReport> ModelLearningReports { get; set; }
        public DbSet<ModelLearningConf> ModelLearningConfs { get; set; }
        public DbSet<ModelLearningMarkup> ModelLearningMarkups { get; set; }
        public DbSet<Partition> Partitions { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<PatternWordRel> PatternWordRels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordForm> WordForms { get; set; }
        public DbSet<Config> Configs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var entity = builder.Entity<PatternWordRel>().ToTable("PatternWordRel", "dbo");
            entity.HasKey(e => new {e.PatternId, e.WordId});

            var entityRel = builder.Entity<BirthdayException>().ToTable("BirthdayException", "dbo");
            entityRel.HasKey(e => e.EmployeeId);

            builder.ConfugueViewEntityInt<Category>();
            builder.ConfugueViewEntityInt<History>();
            builder.ConfugueViewEntityInt<Learning>();
            builder.ConfugueViewEntity<ModelLearning>();
            builder.ConfugueViewEntityInt<ModelLearningReport>();
            builder.ConfugueViewEntityInt<ModelLearningConf>();
            builder.ConfugueViewEntityInt<ModelLearningMarkup>();
            builder.ConfugueViewEntity<Partition>();
            builder.ConfugueViewEntityInt<Pattern>();
            builder.ConfugueViewEntityInt<Word>();
            builder.ConfugueViewEntityInt<WordForm>();
            builder.ConfugueDboEntity<Config>(x => x.Name);
        }


    }

}
