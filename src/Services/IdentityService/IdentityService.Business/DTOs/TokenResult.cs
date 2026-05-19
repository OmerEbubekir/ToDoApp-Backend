namespace IdentityService.Business.DTOs;

public class TokenResult
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}