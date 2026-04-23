using Products.Src.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Products.Src.Infrastructure.Middleware;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception caught: {Message}", exception.Message);

        var (statusCode, title, detail) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; 
    }

    private static (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            DomainException domainEx => (StatusCodes.Status400BadRequest, "Error de business", domainEx.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not found", exception.Message),
            TechnicalException techEx => (StatusCodes.Status500InternalServerError, "Error server", techEx.Message),
            _ => (StatusCodes.Status500InternalServerError, "Error", "Error request.")
        };
    }
}