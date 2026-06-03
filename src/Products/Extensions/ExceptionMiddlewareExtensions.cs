using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Products.Extensions;

internal sealed class CustomExceptionHandler(ILogger logger) : IExceptionHandler
{ 
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.Error($"Exception Filter: {exception}");
        
        var status = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        
        httpContext.Response.StatusCode = status;

        var problemDetails = new ProblemDetails
        { 
            Status = status,
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}