namespace ToDoService.API.Models;
public class CreateToDoRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}