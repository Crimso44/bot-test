using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions.ChatBot
{
    public interface IGetChatBotSubpartitionDictionaryCollection : IQuery<GetCollectionSpecification, CollectionDto<DictionaryItemDto>>
    {
    }
}
