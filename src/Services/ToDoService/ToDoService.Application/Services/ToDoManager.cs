using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Contexts;
using Shared.Data.Entities;
using ToDoService.Application.DTOs;
using ToDoService.Application.Interfaces;

namespace ToDoService.Application.Services;

public class ToDoManager : IToDoService
{
    private readonly ToDoDbContext _context;
    private readonly IMapper _mapper;

    public ToDoManager(ToDoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ToDoResponse>> GetAllByUserIdAsync(string userId)
    {
        var items = await _context.ToDoItems.Where(x => x.UserId == userId).ToListAsync();

        
        return _mapper.Map<List<ToDoResponse>>(items);
    }

    public async Task<ToDoResponse> CreateAsync(CreateToDoRequest request, string userId)
    {
        
        var item = _mapper.Map<ToDoItem>(request);

        
        item.Id = Guid.NewGuid();
        item.UserId = userId;
        item.CreatedAt = DateTime.UtcNow;
        item.IsCompleted = false;

        await _context.ToDoItems.AddAsync(item);
        await _context.SaveChangesAsync();

        return _mapper.Map<ToDoResponse>(item);
    }

    public async Task<ToDoResponse?> UpdateAsync(Guid id, UpdateToDoRequest request, string userId)
    {
        var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (item == null) return null;

        
        _mapper.Map(request, item);

        _context.ToDoItems.Update(item);
        await _context.SaveChangesAsync();

        return _mapper.Map<ToDoResponse>(item);
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (item == null) return false;

        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}