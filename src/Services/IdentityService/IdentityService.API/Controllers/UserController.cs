using FluentValidation;
using IdentityService.Business.DTOs;
using IdentityService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace IdentityService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    

    public UserController(IAuthService authService)
    {
        _authService = authService;
       
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {   
                     
        var isSuccess = await _authService.RegisterAsync(dto);

        if (!isSuccess)
            return BadRequest(new { message = "Kullanıcı kaydı başarısız oldu. E-posta zaten kullanımda olabilir." });

        return StatusCode(201, new { message = "Kullanıcı başarıyla oluşturuldu." });
    }
}