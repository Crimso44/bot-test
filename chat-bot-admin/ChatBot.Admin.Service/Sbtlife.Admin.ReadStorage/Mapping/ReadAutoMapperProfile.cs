using System;
using AutoMapper;
using ChatBot.Admin.Common.Const.ChatBot;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.ReadStorage.Model;
using ChatBot.Admin.ReadStorage.Contexts.ChatBot;
using ChatBot.Admin.ReadStorage.Model.ChatBot;

namespace ChatBot.Admin.ReadStorage.Mapping
{
    public class ReadAutoMapperProfile : Profile
    {
        public ReadAutoMapperProfile()
        {
            // ChatBot
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.RequiredRoster, a => a.MapFrom(s => s.RequiredRoster.Trim()))
                /*.ForMember(d => d.RequiredRosterName, a => a.MapFrom(s => 
                    ChatBotConst.Roster.Sources.ContainsKey(s.RequiredRoster.Trim()) ?
                        ChatBotConst.Roster.Sources[s.RequiredRoster.Trim()].Name : null
                    ))*/;
            CreateMap<Partition, PartitionDto>();
            CreateMap<Partition, DictionaryItemDto>();
            CreateMap<Pattern, PatternDto>()
                .ForMember(d => d.Words, o => o.Ignore());
            CreateMap<Pattern, PatternsDto>();
            CreateMap<Word, WordDto>();
            CreateMap<WordForm, WordFormDto>();
            CreateMap<Learning, LearningDto>();
            CreateMap<ModelLearning, ModelLearningDto>();
            CreateMap<ModelLearningReport, ModelReportDto>();
            CreateMap<ModelLearningConf, ModelReportConfusionDto>()
                .ForMember(d => d.Confusion, o => o.MapFrom(s => Convert.ToInt32(s.Confusion ?? 0)))
                .ForMember(d => d.CategoryId, o => o.Ignore())
                .ForMember(d => d.OriginId, o => o.MapFrom(s => s.CategoryId));
            CreateMap<History, HistoryDto>();
        }
    }
}
