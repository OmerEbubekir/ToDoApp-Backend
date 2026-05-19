using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Exceptions;
using Shared.Core.Interfaces; 
using ToDoService.Application.DTOs;
using ToDoService.Application.Interfaces;

namespace ToDoService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly ICurrentUserService _currentUserService; 

        public TodoController(IToDoService toDoService, ICurrentUserService currentUserService)
        {
            _toDoService = toDoService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyToDos()
        {
            
            var userId = _currentUserService.User!.Id;
            var items = await _toDoService.GetAllByUserIdAsync(userId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDo([FromBody] CreateToDoDto createToDoDto)
        {
            var userId = _currentUserService.User!.Id;
            var item = await _toDoService.CreateAsync(createToDoDto, userId);
            return StatusCode(201, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(Guid id)
        {
            var userId = _currentUserService.User!.Id;
            var result = await _toDoService.DeleteAsync(id, userId);

            if (!result)
                throw new NotFoundException("Görev bulunamadı veya bu görevi silmeye yetkiniz yok.");

            return Ok(new { message = "Görev başarıyla silindi" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(Guid id, [FromBody] UpdateToDoDto updateToDoDto)
        {
            var userId = _currentUserService.User!.Id;
            var item = await _toDoService.UpdateAsync(id, updateToDoDto, userId);

            if (item == null)
                throw new NotFoundException("Görev bulunamadı veya bu görevi silmeye yetkiniz yok.");

            return Ok(item);
        }
    }
}