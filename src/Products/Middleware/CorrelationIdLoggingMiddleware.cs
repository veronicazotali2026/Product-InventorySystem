using Serilog.Context;

namespace Products.Middleware;

public class CorrelationIdLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICorrelationIdAccessor accessor)
    {
        // Push correlation ID to Serilog's log context
        using (LogContext.PushProperty("CorrelationId", accessor.CorrelationId))
        {
            await next(context);
        }
    }
}