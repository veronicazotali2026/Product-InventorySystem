using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

internal sealed class ProductRepository(RepositoryContext repositoryContext)
	: RepositoryBase<Product>(repositoryContext), IProductRepository
{
	public async Task<Product?> GetProductAsync(Guid productId, bool trackChanges) =>
		await FindByCondition(c => c.Id.Equals(productId), trackChanges)
		.SingleOrDefaultAsync();

	public void CreateProduct(Product product) => Create(product);
}