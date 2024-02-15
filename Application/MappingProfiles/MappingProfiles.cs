using Application.Dtos;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles() 
        {
            CreateMap<ExtendedCmb, ReadCmbRequest>().ReverseMap();
            CreateMap<ExtendedCmb, UpdateCmbRequest>().ReverseMap();
            CreateMap<Cmb, CmbResponse>().ReverseMap();
        }
    }
}
