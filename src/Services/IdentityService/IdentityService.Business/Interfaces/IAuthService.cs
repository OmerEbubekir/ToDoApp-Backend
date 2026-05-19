using IdentityService.Business.Models;

namespace IdentityService.Business.Interfaces;

public interface IAuthService
{
    Task<AuthResult?> LoginAsync(LoginArgs args);
    Task<bool> RegisterAsync(RegisterArgs args);
}