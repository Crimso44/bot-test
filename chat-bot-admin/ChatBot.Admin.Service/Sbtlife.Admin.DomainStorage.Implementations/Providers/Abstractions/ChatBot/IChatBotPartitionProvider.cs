using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;

namespace ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot
{
    public interface IChatBotPartitionProvider
    {
        bool CheckExistsAndNotDeleted(Guid id);
        bool CheckHasLinks(Guid id);
        bool CheckHasSubpartitions(Guid id);
        bool CheckCaptionUnique(Guid partId, string caption);
        bool CheckCaptionUnique(string caption);
        void AddPartition(PartitionDto partition);
        void ModifyPartition(PartitionOptionalDto partition);
        void DeletePartition(Guid id);
    }
}
