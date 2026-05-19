using AutoMapper;
using IdentityService.Business.Models;
using Shared.Data.Entities;

namespace IdentityService.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<RegisterArgs, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
    }
}