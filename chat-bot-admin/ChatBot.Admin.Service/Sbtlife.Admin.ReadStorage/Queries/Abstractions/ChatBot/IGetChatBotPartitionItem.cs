using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetChatBotPartitionItem : IQuery<GetItemSpecification, PartitionDto>
    {
    }
}
