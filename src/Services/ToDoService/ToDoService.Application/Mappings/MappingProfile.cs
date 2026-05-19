using AutoMapper;
using Shared.Data.Entities;
using ToDoService.Application.DTOs;

namespace ToDoService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<CreateToDoRequest, ToDoItem>();
        CreateMap<UpdateToDoRequest, ToDoItem>();

        
        CreateMap<ToDoItem, ToDoResponse>();
    }
}   