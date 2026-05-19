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
        // 1. Determine the status code based on the exception type
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ValidationAppException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        // 2. Log the error appropriately
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Critical System Error: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Client Error ({StatusCode}): {Message}", statusCode, exception.Message);
        }

        // 3. Construct the error response
        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = exception.Message,
            ValidationErrors = exception is ValidationAppException ve ? ve.Errors : null,
            Details = statusCode == StatusCodes.Status500InternalServerError ? "An internal server error occurred." : null
        };

        // 4. Send the response to the client
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}