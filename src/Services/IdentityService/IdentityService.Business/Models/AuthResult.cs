namespace IdentityService.Business.Models;

public class AuthResult
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}