using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetChatBotPartitionDictionaryCollection : IQuery<GetCollectionSpecification, CollectionDto<DictionaryItemDto>>
    {
    }
}
