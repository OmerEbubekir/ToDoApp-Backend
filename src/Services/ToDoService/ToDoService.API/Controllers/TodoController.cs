using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public TodoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetMyToDos()
        {
            var userId = GetUserId();
            var items = await _toDoService.GetAllByUserIdAsync(userId);
            return Ok(items);
        }
        [HttpPost]
        public async Task<IActionResult> CreatateToDo([FromBody] CreateToDoDto createToDoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var item = await _toDoService.CreateAsync(createToDoDto, userId);
            return StatusCode(201, item);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(Guid id)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var result = await _toDoService.DeleteAsync(id, userId);

            if (!result)
                return NotFound(new { message = "Görev bulunamadı.." });

            return Ok(new { message = "Görev başarıyla silindi" });


        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(Guid id, [FromBody] UpdateToDoDto updateToDoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var item = await _toDoService.UpdateAsync(id, updateToDoDto, userId);
            if (item == null)
                return NotFound(new { message = "Görev bulunamadı.." });
            return Ok(item);

        }
    }
}
