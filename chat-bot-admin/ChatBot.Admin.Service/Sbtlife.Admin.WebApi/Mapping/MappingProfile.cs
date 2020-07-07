using AutoMapper;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Specifications;
using ChatBot.Admin.ReadStorage.Specifications.ChatBot;
using ChatBot.Admin.WebApi.Requests;
using ChatBot.Admin.WebApi.ViewModel.ChatBot;

namespace ChatBot.Admin.WebApi.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DictionaryItemDto, ViewModel.DictionaryItemDto>();
            CreateMap(typeof(CollectionDto<>), typeof(ViewModel.CollectionDto<>));

            CreateMap<GetCollectionFilter, GetCollectionSpecification>();
            CreateMap<GetCategoryCollectionFilter, GetCategoryCollectionSpecification>();
            CreateMap<GetHistoryFilter, GetHistorySpecification>();
            CreateMap<GetLearningFilter, GetLearningSpecification>();
            CreateMap<GetPatternsFilter, GetPatternsSpecification>();
            CreateMap<GetModelFilter, GetModelSpecification>();

            CreateMap<HistoryDto, ChatBotHistoryDto>();
        }
    }
}
