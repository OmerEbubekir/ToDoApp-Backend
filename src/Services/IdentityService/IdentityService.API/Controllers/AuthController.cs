using FluentValidation;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Exceptions;

namespace IdentityService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginDto> _validator;
        public AuthController(IAuthService authService, IValidator<LoginDto> validator)
        {
            _authService = authService;
            _validator = validator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid) {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
               throw new ValidationAppException(errors);
            }
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(token);

        }
    }
}
