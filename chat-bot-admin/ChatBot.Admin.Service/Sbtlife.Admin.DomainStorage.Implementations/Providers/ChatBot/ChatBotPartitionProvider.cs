using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.Common.Extensions;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Contexts;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot;

namespace ChatBot.Admin.DomainStorage.Providers.ChatBot
{
    class ChatBotPartitionProvider : ProviderChatBot, IChatBotPartitionProvider
    {
        public ChatBotPartitionProvider(ChatBotContext storage)
            : base(storage)
        {
        }

        public  bool CheckExistsAndNotDeleted(Guid id)
        {
            return  Context.Partitions.Any(g => g.Id == id);
        }

        public  bool CheckHasLinks(Guid id)
        {
            return  Context.Categorys.Join(Context.Partitions, c => c.PartitionId, p => p.Id, (c, p) => new {c,p})
                .Any(l => l.c.PartitionId == id || l.p.ParentId == id);
        }

        public  bool CheckHasSubpartitions(Guid id)
        {
            return  Context.Partitions.Any(s => s.ParentId == id);
        }

        public  bool CheckCaptionUnique(string caption)
        {
            return ! Context.Partitions.Any(g => g.Name == caption && g.ParentId == null);
        }


        public  bool CheckCaptionUnique(Guid partId, string caption)
        {
            return ! Context.Partitions.Any(g => g.Name == caption && g.ParentId == partId);
        }


        public  void AddPartition(PartitionDto partition)
        {
             Context.Partitions.Add(new Partition
            {
                Id = partition.Id,
                Name = partition.Title,
                ParentId = partition.ParentId
            });
             Context.SaveChanges();
        }

        public  void ModifyPartition(PartitionOptionalDto partition)
        {
            var entity =  GetPartitionRaw(partition.Id);

            OptionalHelper.SafeUpdate(v => entity.Name = v, partition.Title);
            OptionalHelper.SafeUpdate(v => entity.ParentId = v, partition.ParentId);
             Context.SaveChanges();
        }

        public  void DeletePartition(Guid id)
        {
            var entity =  GetPartitionRaw(id);

            if (entity == null)
                throw new ArgumentException(nameof(id));

            Context.Partitions.Remove(entity);
             Context.SaveChanges();
        }

        private  Partition GetPartitionRaw(Guid id)
        {
            return  Context.Partitions.SingleOrDefault(l => l.Id == id);
        }
    }
}
