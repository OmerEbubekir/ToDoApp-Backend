namespace ToDoService.Business.Models;

public class CreateToDoArgs
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}