using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Core.Interfaces;
using Shared.Core.Models;

namespace Shared.Core.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    
    public CurrentUser? User
    {
        get
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;

            if (userPrincipal?.Identity?.IsAuthenticated != true)
                return null;

            var id = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var email = userPrincipal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

                
            var roles = userPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return new CurrentUser
            {
                Id = id,
                Email = email,
                Roles = roles
            };
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}