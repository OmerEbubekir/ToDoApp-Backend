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
        // 1. Hatayı Terminale/Log'a yazdır
        _logger.LogError(exception, "Sistemde bir hata meydana geldi: {Message}", exception.Message);

        // 2. Hataya göre Status Code belirle
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError // Bilinmeyen hatalar için
        };

        // 3. Standart JSON yanıtımızı oluştur
        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = exception.Message,
            // Sadece Internal Server Error ise stack trace'i gizle (güvenlik için)
            Details = statusCode == 500 ? "Sunucu içi bir hata oluştu." : null
        };

        // 4. Yanıtı istemciye (kullanıcıya/frontend'e) gönder
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        // true dönerek ".NET sen karışma, hatayı ben ele aldım" diyoruz.
        return true;
    }
}