using IdentityService.Business.DTOs;

namespace IdentityService.Business.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest dto);
    Task<TokenResult?> LoginAsync(LoginRequest dto);
}