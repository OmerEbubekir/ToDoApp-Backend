using AutoMapper;
using IdentityService.API.Models;
using IdentityService.Business.Models;


namespace IdentityService.API.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        
        CreateMap<LoginRequest, LoginArgs>();
        CreateMap<RegisterRequest, RegisterArgs>();

        
        CreateMap<AuthResult, AuthResponse>();
    }
}