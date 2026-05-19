using IdentityService.Business.DTOs;
using IdentityService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        
        var token = await _authService.LoginAsync(dto);

        if (token == null)
            return Unauthorized(new { message = "E-posta veya şifre hatalı." });

        return Ok(token);
    }
}