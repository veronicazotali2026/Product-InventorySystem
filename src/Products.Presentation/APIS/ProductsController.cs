using System;
using System.Threading;
using System.Threading.Tasks;
using Entities.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Presentation.ActionFilters;
using Products.Services;
using Shared.DataTransferObjects;
using Shared.Response;

using Serilog;
using Shared.Extensions;

namespace Products.Presentation.APIS;

[Route("api/products")]
public class ProductsController(IServiceManager service, ILogger logger) : ApiControllerBase
{

    [HttpGet("{id:guid}", Name = "Get")]
    [ProducesResponseType(typeof(ApiBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(Guid id)
    {
        logger.Information("Requesting Product: {ProductId}", id);
        var baseResult = await service.ProductService.GetProductIdAsync(id, CancellationToken.None);
            
        if (!baseResult.Success)
            return ProcessError(baseResult);
            
        return Ok(baseResult);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Post([FromBody] CreateProductCommand cmd)
    {
        logger.Information("Creating Product: {ProductId}",cmd.Name);
        
        var baseResult = await service.ProductService.SaveProductAsync(cmd, CancellationToken.None);
        var productResult = baseResult.GetResult<ProductResponse>();
        return CreatedAtRoute("Get", new { id = productResult.Id }, productResult);
    }
}