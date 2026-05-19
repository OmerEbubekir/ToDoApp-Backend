
using Shared.Data.Entities;
using ToDoService.Application.DTOs;

namespace ToDoService.Application.Interfaces
{
    public interface IToDoService
    {
        Task<List<ToDoItem>> GetAllByUserIdAsync(string userId);
        Task<ToDoItem> CreateAsync(CreateToDoDto createToDoDto, string userId);
        Task<ToDoItem?> UpdateAsync(Guid id, UpdateToDoDto updateToDoDto, string userId);
        Task<bool> DeleteAsync(Guid id, string userId);
    }
}
