using System;

namespace Shared.Data.Entities;

    public class ToDoItem
    {
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relational Properties (Hangi kullanıcıya ait?)
    public string UserId { get; set; } = string.Empty;
    
    }