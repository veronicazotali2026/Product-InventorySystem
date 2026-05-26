namespace Products.Middleware;

public interface ICorrelationIdAccessor
{
    string CorrelationId { get; set; }
}