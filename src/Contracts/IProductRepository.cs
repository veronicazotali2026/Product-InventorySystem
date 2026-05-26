using Entities.Models;

namespace Contracts;

public interface IProductRepository
{
	Task<Product?> GetProductAsync(Guid productId, bool trackChanges);
	void CreateProduct(Product product);
}
