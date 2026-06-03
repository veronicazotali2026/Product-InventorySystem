using Contracts;
using Entities.Models;
using Entities.Responses;
using Serilog;
using Shared.DataTransferObjects;
using Shared.Response;

namespace Products.Services;

public class ProductService(IRepositoryManager repository, IInventoryServiceClient inventoryServiceClient, ILogger logger) : IProductService
{
    public async Task<ApiBaseResponse> SaveProductAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Could use Automapper or some library to handle mapping (It does not need to be Automapper!!).
        var product = new Product()
        {
            Description = request.Description,
            Name = request.Name
        };
        
        repository.Product.CreateProduct(product);
        
        await repository.SaveAsync();
        
        logger.Information($"DB: Saved product:{request.Name}.");

        var productDto = new ProductResponse(product.Id);

        return new ApiOkResponse<ProductResponse>(productDto);
    }
    
    public async Task<ApiBaseResponse> GetProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await repository.Product.GetProductAsync(productId, true);

        if (product is null)
        {
            logger.Information($"DB: ProductId not found:{productId}.");
            return new ProductNotFoundResponse(productId);
        }

        var inventoryResponse = await GetProductFromInventoryAsync(productId);

        if (inventoryResponse.product == null || !inventoryResponse.result)
        {
            logger.Error($"Inventory service unavailable whilst trying to get information for product:{productId}");
            
            return new ApiOkResponse<EnrichedProductWithMessage>(new EnrichedProductWithMessage
            {
                Id = productId,
                Description = product.Description,
                Name = product.Name
            });
        }

        logger.Information($"Successfully received information, from Inventory service for product:{productId}");

        var pricingModel = inventoryResponse.product;
        
        var enrichedProduct = new EnrichedProductWithPricing()
        {
            Id = productId,
            Description = product.Description,
            Name = product.Name,
            Price = pricingModel.Price,
            Stock = pricingModel.Stock
        };

        return new ApiOkResponse<EnrichedProductWithPricing>(enrichedProduct);
    }

     async Task<(InventoryProduct? product,bool result)> GetProductFromInventoryAsync(Guid productId)
     {
         try
         {
             var inventoryResponse = await inventoryServiceClient.GetProductAsync(productId);

             return inventoryResponse.IsSuccessful ? new ValueTuple<InventoryProduct, bool>(inventoryResponse.Content, true) : new ValueTuple<InventoryProduct?, bool>(null, false);
         }
         catch (Exception ex)
         {
             //Another way to handle it is via APIException(refit), but regardless, we need to ensure flow continues, so we can return local product.
             logger.Error($"Inventory service unavailable whilst trying to get information for product:{productId}", ex);
             return new ValueTuple<InventoryProduct?, bool>(null, false);
         }
     }
}