namespace Shared.DataTransferObjects;

public record InventoryProduct
{
    public Guid Id { get; init; }
    public decimal? Price { get; init; }
    public decimal? Stock { get; init; }
}