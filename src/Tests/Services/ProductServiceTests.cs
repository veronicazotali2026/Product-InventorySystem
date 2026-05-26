using Contracts;
using Entities.Models;
using Entities.Responses;
using Moq;
using Products.Services;
using Refit;
using Serilog;
using Shared.DataTransferObjects;
using Shared.Extensions;

namespace Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IRepositoryManager> _repositoryManager = new();
    private readonly Mock<IInventoryServiceClient> _inventoryServiceClient = new();
    private readonly Mock<ILogger> _logger = new();

    [Fact]
    public async Task GetProductIdAsync_Should_Return_ProductNotFoundResponse_When_Product_Id_Is_Not_Found()
    {
        //ARRANGE
        _repositoryManager.Setup(x => x.Product.GetProductAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult<Product?>(null));
        
        //ACT
        var productId = Guid.NewGuid();
        var productService = new ProductService(_repositoryManager.Object, _inventoryServiceClient.Object, _logger.Object);
        var result = await productService.GetProductIdAsync(productId, new CancellationToken());
        
        //ASSERT
        Assert.True(result is ProductNotFoundResponse);
    }
    
    [Fact]
    public async Task GetProductIdAsync_Should_Return_EnrichedProductId_Without_PricingModel()
    {
        //ARRANGE
        var productId = Guid.NewGuid();
        var existingProduct = new Product()
        {
            Id = productId,
            Description = "Laptop HP",
            Name = "Laptop",
        };
        
        _repositoryManager.Setup(x => x.Product.GetProductAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult<Product>(existingProduct)!);
        _inventoryServiceClient.Setup(x => x.GetProductAsync(productId)).Throws(new Exception());
        
        //ACT
        var productService = new ProductService(_repositoryManager.Object, _inventoryServiceClient.Object, _logger.Object);
        var result = await productService.GetProductIdAsync(productId, new CancellationToken());
        var baseResult = result.GetResult<EnrichedProduct>();
        var priceDetails = baseResult.PriceDetails;
        
        //ASSERT
        Assert.True(((PriceDetailsFailure)priceDetails).Message == "Data Unavailable");
    }
}