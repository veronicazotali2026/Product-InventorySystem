using Entities.Responses;
using Shared.DataTransferObjects;

namespace Products.Services;

public interface IProductService
{
    Task<ApiBaseResponse> SaveProductAsync(CreateProductCommand request, CancellationToken cancellationToken);
    Task<ApiBaseResponse> GetProductIdAsync(Guid productId, CancellationToken cancellationToken);
}