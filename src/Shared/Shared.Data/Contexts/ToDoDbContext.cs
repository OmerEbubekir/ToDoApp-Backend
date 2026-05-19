using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace Shared.Data.Contexts;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<ToDoItem>(entity =>
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        });
    }
}