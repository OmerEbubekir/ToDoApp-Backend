using ToDoService.Business.DTOs;

namespace ToDoService.Business.Interfaces;

public interface IToDoService
{
    
    Task<List<ToDoResponse>> GetAllByUserIdAsync(string userId);
    Task<ToDoResponse> CreateAsync(CreateToDoRequest request, string userId);
    Task<ToDoResponse?> UpdateAsync(Guid id, UpdateToDoRequest request, string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
}