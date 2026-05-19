using ToDoService.Business.Models;

namespace ToDoService.Business.Interfaces;

public interface IToDoService
{
    Task<List<ToDoResult>> GetAllByUserIdAsync(string userId);
    Task<ToDoResult> CreateAsync(CreateToDoArgs args, string userId);
    Task<ToDoResult?> UpdateAsync(Guid id, UpdateToDoArgs args, string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
}