using Microsoft.EntityFrameworkCore;
using Shared.Data.Contexts;
using Shared.Data.Entities;
using ToDoService.Application.DTOs;
using ToDoService.Application.Interfaces;

namespace ToDoService.Application.Services
{
    public class ToDoManager : IToDoService
    {
        private readonly ToDoDbContext _context;

        public ToDoManager(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoItem> CreateAsync(CreateToDoDto createToDoDto, string userId)
        {
            var item = new ToDoItem
            {
                Id = Guid.NewGuid(),
                Title = createToDoDto.Title,
                Description = createToDoDto.Description,
                IsCompleted = false,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await _context.ToDoItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(Guid id, string userId)
        {
            var item =await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (item == null)
                return false;
            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ToDoItem>> GetAllByUserIdAsync(string userId)
        {
            return await _context.ToDoItems.Where(x=>x.UserId==userId).ToListAsync();
        }

        public async Task<ToDoItem> UpdateAsync(Guid id, UpdateToDoDto updateToDoDto, string userId)
        {
            var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (item == null)
                return null;

            item.IsCompleted = updateToDoDto.IsCompleted;
            item.Title = updateToDoDto.Title;
            item.Description = updateToDoDto.Description;
            item.CreatedAt = DateTime.Now;

            _context.ToDoItems.Update(item);
            await _context.SaveChangesAsync();
            return item;

        }
    }
}
