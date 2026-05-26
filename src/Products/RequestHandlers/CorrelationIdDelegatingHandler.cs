using Products.Middleware;

namespace Products.RequestHandlers;

public class CorrelationIdDelegatingHandler(ICorrelationIdAccessor accessor) : DelegatingHandler
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Add correlation ID to outgoing request if we have one
        if (!string.IsNullOrEmpty(accessor.CorrelationId))
        {
            request.Headers.TryAddWithoutValidation(CorrelationIdHeader, accessor.CorrelationId);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}