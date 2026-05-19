using FluentValidation;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Exceptions;

namespace IdentityService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    

    public UserController(IAuthService authService, IValidator<RegisterDto> validator)
    {
        _authService = authService;
       
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
                     
        var isSuccess = await _authService.RegisterAsync(dto);

        if (!isSuccess)
            return BadRequest(new { message = "Kullanıcı kaydı başarısız oldu. E-posta zaten kullanımda olabilir." });

        return StatusCode(201, new { message = "Kullanıcı başarıyla oluşturuldu." });
    }
}