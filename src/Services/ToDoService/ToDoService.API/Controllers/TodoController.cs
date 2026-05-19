using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Exceptions;
using Shared.Core.Interfaces;
using ToDoService.API.Models;
using ToDoService.Business.Interfaces;
using ToDoService.Business.Models;

namespace ToDoService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper; // AutoMapper sahneye çıkıyor

        public TodoController(IToDoService toDoService, ICurrentUserService currentUserService, IMapper mapper)
        {
            _toDoService = toDoService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyToDos()
        {
            var userId = _currentUserService.User!.Id;
            var results = await _toDoService.GetAllByUserIdAsync(userId);

            // Result listesini Response listesine tek satırda çeviriyoruz!
            var response = _mapper.Map<List<ToDoResponse>>(results);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDo([FromBody] CreateToDoRequest request)
        {
            // 1. Request -> Args dönüşümü
            var args = _mapper.Map<CreateToDoArgs>(request);

            var userId = _currentUserService.User!.Id;
            var result = await _toDoService.CreateAsync(args, userId);

            // 2. Result -> Response dönüşümü
            var response = _mapper.Map<ToDoResponse>(result);

            return StatusCode(201, response);
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
        public async Task<IActionResult> UpdateToDo(Guid id, [FromBody] UpdateToDoRequest request)
        {
            // 1. Request -> Args dönüşümü
            var args = _mapper.Map<UpdateToDoArgs>(request);

            var userId = _currentUserService.User!.Id;
            var result = await _toDoService.UpdateAsync(id, args, userId);

            if (result == null)
                throw new NotFoundException("Task not found or you do not have permission to update it.");

            // 2. Result -> Response dönüşümü
            var response = _mapper.Map<ToDoResponse>(result);

            return Ok(response);
        }
    }
}