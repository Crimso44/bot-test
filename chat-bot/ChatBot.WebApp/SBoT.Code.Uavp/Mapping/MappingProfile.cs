using AutoMapper;
using SBoT.Code.Uavp.DataModel.Cross;
using SBoT.Code.Uavp.Dto;

namespace SBoT.Code.Uavp.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffDto>();
        }
    }
}
