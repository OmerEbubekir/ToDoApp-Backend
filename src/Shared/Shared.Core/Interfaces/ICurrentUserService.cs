using Shared.Core.Models;

namespace Shared.Core.Interfaces;

public interface ICurrentUserService
{
    
    CurrentUser? User { get; }
    bool IsAuthenticated { get; }
}