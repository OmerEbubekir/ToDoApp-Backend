namespace ToDoService.Business.Models;
public class UpdateToDoArgs
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}