namespace Products.Middleware;

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    public string CorrelationId { get; set; } = string.Empty;
}