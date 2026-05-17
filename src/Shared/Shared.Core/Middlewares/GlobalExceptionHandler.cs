using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Core.Exceptions;
using Shared.Core.Models;

namespace Shared.Core.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ValidationAppException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            
            _logger.LogError(exception, "Kritik Sistem Hatası: {Message}", exception.Message);
        }
        else
        {
            
            _logger.LogWarning("İstemci Hatası ({StatusCode}): {Message}", statusCode, exception.Message);
        }

        
        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = exception.Message,
            ValidationErrors = exception is ValidationAppException ve ? ve.Errors : null,
            Details = statusCode == StatusCodes.Status500InternalServerError ? "Sunucu içi bir hata oluştu." : null
        };

        // 4. Yanıtı istemciye (kullanıcıya/frontend'e) gönder
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}