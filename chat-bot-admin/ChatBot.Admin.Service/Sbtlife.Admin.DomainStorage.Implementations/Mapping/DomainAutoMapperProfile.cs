using AutoMapper;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot;

namespace ChatBot.Admin.DomainStorage.Mapping
{
    public class DomainAutoMapperProfile : Profile
    {
        public DomainAutoMapperProfile()
        {
            // ChatBot
            CreateMap<Category, CategoryDto>();
            CreateMap<Partition, PartitionDto>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Name));
            CreateMap<LearningDto, Learning>();
            CreateMap<Learning, LearningDto>();
            CreateMap<Pattern, PatternDto>()
                .ForMember(d => d.Words, o => o.Ignore());

            CreateMap<ModelLearningDto, ModelLearning>();
            CreateMap<ModelLearning, ModelLearningDto>();
        }
    }
}
