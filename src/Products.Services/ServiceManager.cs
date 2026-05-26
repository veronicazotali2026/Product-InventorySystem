using Contracts;
using Serilog;

namespace Products.Services;

public sealed class ServiceManager(IRepositoryManager repositoryManager, IInventoryServiceClient client, ILogger logger) : IServiceManager
{
	private readonly Lazy<IProductService> _productService = new(() => new ProductService(repositoryManager, client, logger));

	public IProductService ProductService => _productService.Value;
}