using CurrencyConverter.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Middleware;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("Exception occurred: {@exception}", exception.GetOriginalException().Message);

        var problemDetails = new ValidationProblemDetails
        {
            Type = exception.GetType().Name,
            Title = "Error occurred",
            Status = StatusCodes.Status500InternalServerError,
            Errors = new Dictionary<string, string[]> { { "Message", [exception.Message] } },
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}