namespace IdentityService.Application.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}