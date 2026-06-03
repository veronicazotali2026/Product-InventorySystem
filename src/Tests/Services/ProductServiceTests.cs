using System.Net;
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
        var result = await productService.GetProductIdAsync(productId, CancellationToken.None);
        
        //ASSERT
        Assert.True(result is ProductNotFoundResponse);
    }
    
    [Fact]
    public async Task GetProductIdAsync_Should_Return_EnrichedProduct_Without_Message()
    {
        //ARRANGE
        var productId = Guid.NewGuid();
        var existingProduct = new Product()
        {
            Id = productId,
            Description = "Laptop HP",
            Name = "Laptop",
        };
        
        _repositoryManager.Setup(x => x.Product.GetProductAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult(existingProduct)!);
        _inventoryServiceClient.Setup(x => x.GetProductAsync(productId)).Throws(new Exception());
        
        //ACT
        var productService = new ProductService(_repositoryManager.Object, _inventoryServiceClient.Object, _logger.Object);
        var result = await productService.GetProductIdAsync(productId, CancellationToken.None);
        var baseResult = result.GetResult<EnrichedProductWithMessage>();
        
        //ASSERT
        Assert.True(baseResult.Message == "Data unavailable");
    }
    
    [Fact]
    public async Task GetProductIdAsync_Should_Return_EnrichedProduct_With_PricingModel()
    {
        //ARRANGE
        var productId = Guid.NewGuid();
        var existingProduct = new Product()
        {
            Id = productId,
            Description = "Laptop HP",
            Name = "Laptop",
        };
        
        var apiResponse = new ApiResponse<InventoryProduct>(
            new HttpResponseMessage(HttpStatusCode.OK),
            new InventoryProduct()
            {
                Price = 1000m,
                Stock = 200m
            },null!);
        
        _repositoryManager.Setup(x => x.Product.GetProductAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult(existingProduct)!);
        _inventoryServiceClient.Setup(x => x.GetProductAsync(productId))
            .ReturnsAsync(apiResponse);
        
        var productService = new ProductService(_repositoryManager.Object, _inventoryServiceClient.Object, _logger.Object);
        var result = await productService.GetProductIdAsync(productId, CancellationToken.None);
        var baseResult = result.GetResult<EnrichedProductWithPricing>();
        
        //ASSERT
        Assert.True(baseResult.Price == 1000m);
        Assert.True(baseResult.Stock == 200m);
    }
}