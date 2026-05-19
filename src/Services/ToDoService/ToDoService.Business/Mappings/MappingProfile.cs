using AutoMapper;
using Shared.Data.Entities;
using ToDoService.Business.Models;

namespace ToDoService.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<CreateToDoArgs, ToDoItem>();
        CreateMap<UpdateToDoArgs, ToDoItem>();

        
        CreateMap<ToDoItem, ToDoResult>();
    }
}   