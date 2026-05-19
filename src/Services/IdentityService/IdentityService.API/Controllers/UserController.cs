using AutoMapper;
using FluentValidation;
using IdentityService.API.Models;
using IdentityService.Business.Interfaces;
using IdentityService.Business.Models;
using Microsoft.AspNetCore.Mvc;


namespace IdentityService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public UserController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
      
        var args = _mapper.Map<RegisterArgs>(request);

        var result = await _authService.RegisterAsync(args);

        if (!result)
            return BadRequest(new { message = "User creation failed." });

        return StatusCode(201, new { message = "User successfully created." });
    }
}