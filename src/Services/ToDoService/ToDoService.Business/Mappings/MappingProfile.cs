using AutoMapper;
using Shared.Data.Entities;
using ToDoService.Business.DTOs;

namespace ToDoService.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<CreateToDoRequest, ToDoItem>();
        CreateMap<UpdateToDoRequest, ToDoItem>();

        
        CreateMap<ToDoItem, ToDoResponse>();
    }
}   