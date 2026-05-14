using Microsoft.AspNetCore.Identity;

namespace Shared.Data.Entities;

public class AppUser : IdentityUser
{
    // IdentityUser zaten Id, Email, UserName, PasswordHash gibi özellikleri barındırır
    public string FullName { get; set; } = string.Empty;
}