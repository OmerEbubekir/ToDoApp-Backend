using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace Shared.Data.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // İlişki Kuralları (Fluent API)
        builder.Entity<ToDoItem>()
            .HasOne(t => t.User)
            .WithMany() // Bir kullanıcının birden fazla ToDo'su olabilir
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse ToDo'ları da silinsin
    }
}