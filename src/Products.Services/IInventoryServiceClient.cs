using Refit;
using Shared.DataTransferObjects;

namespace Products.Services;

public interface IInventoryServiceClient
{
    [Get("/api/inventory/{productId}")]
    Task<IApiResponse<InventoryProduct>> GetProductAsync(Guid productId);
}