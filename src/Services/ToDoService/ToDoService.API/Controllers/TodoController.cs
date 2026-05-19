using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Exceptions;
using Shared.Core.Interfaces; 
using ToDoService.Business.DTOs;
using ToDoService.Business.Interfaces;

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
        public async Task<IActionResult> CreateToDo([FromBody] CreateToDoRequest CreateToDoRequest)
        {
            var userId = _currentUserService.User!.Id;
            var item = await _toDoService.CreateAsync(CreateToDoRequest, userId);
            return StatusCode(201, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(Guid id)
        {
            var userId = _currentUserService.User!.Id;
            var result = await _toDoService.DeleteAsync(id, userId);

            if (!result)
                throw new NotFoundException("Task not found or you do not have permission to delete it.");

            return Ok(new { message = "Task successfully deleted." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(Guid id, [FromBody] UpdateToDoRequest UpdateToDoRequest)
        {
            var userId = _currentUserService.User!.Id;
            var item = await _toDoService.UpdateAsync(id, UpdateToDoRequest, userId);

            if (item == null)
                throw new NotFoundException("Task not found or you do not have permission to update it."); 

            return Ok(item);
        }
    }
}