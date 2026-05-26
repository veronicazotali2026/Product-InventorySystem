namespace Shared.DataTransferObjects;

public record EnrichedProduct
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }

    public PriceDetails PriceDetails { get; init; } = null!;
}

public abstract record PriceDetails 
{
    
}

public record PriceDetailsSuccess: PriceDetails
{
    public decimal? Price { get; init; }
    public decimal? Stock { get; init; }
}

public record PriceDetailsFailure : PriceDetails
{
    public string Message => "Data Unavailable";
}