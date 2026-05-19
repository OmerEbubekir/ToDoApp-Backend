using AutoMapper;
using IdentityService.API.Models;
using IdentityService.Business.Interfaces;
using IdentityService.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Request -> Args
        var args = _mapper.Map<LoginArgs>(request);

        var result = await _authService.LoginAsync(args);

        if (result == null)
            return Unauthorized(new { message = "Invalid email or password." });

        // 2. Result -> Response
        var response = _mapper.Map<AuthResponse>(result);

        return Ok(response);
    }

   
}