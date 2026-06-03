namespace Shared.DataTransferObjects;

public record BaseEnrichedProduct
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}

public record EnrichedProductWithPricing: BaseEnrichedProduct
{
    public decimal? Price { get; init; }
    public decimal? Stock { get; init; }
}

public record EnrichedProductWithMessage : BaseEnrichedProduct
{
    public string Message => "Data unavailable";
}