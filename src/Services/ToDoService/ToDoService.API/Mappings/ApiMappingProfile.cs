using AutoMapper;
using ToDoService.API.Models;
using ToDoService.Business.Models;

namespace ToDoService.API.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // Gelen Request'leri -> Business'ın anladığı Args'lara çevir
        CreateMap<CreateToDoRequest, CreateToDoArgs>();
        CreateMap<UpdateToDoRequest, UpdateToDoArgs>();

        // Business'tan dönen Result'ları -> Kullanıcının göreceği Response'lara çevir
        CreateMap<ToDoResult, ToDoResponse>();
    }
}