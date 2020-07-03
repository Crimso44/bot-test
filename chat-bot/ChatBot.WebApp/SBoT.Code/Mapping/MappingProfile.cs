using AutoMapper;
using SBoT.Code.Dto;
using SBoT.Domain.DataModel.SBoT;

namespace SBoT.Code.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pattern, PatternDto>();
            CreateMap<Word, WordDto>();
            CreateMap<WordForm, WordFormDto>();
            CreateMap<Report, ReportDto>();
            CreateMap<ReportStat, ReportStatDto>();
            CreateMap<History, HistoryDto>();
        }
    }
}
