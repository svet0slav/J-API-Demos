using Microsoft.AspNetCore.Mvc;
using MockyProducts.Shared.Requests;
using MockyProducts.Shared.Responses;
using MockyProducts.Shared.ServiceRequests;
using MockyProducts.Shared.ServiceRequests.Mappers;
using MockyProducts.Shared.Services;

namespace MockyProducts.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IMockyProductsService _service;

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMockyProductsService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("filter")]
        [MapToApiVersion("1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductsDtoResponse>> GetQuery(
            string? minPrice, string? maxPrice, string? size, string? highLight, CancellationToken cancellationToken)
        {
            GetProductsRequest? request = new GetProductsRequest()
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Size = size,
                Highlight = highLight
            };
            _logger.LogInformation("Request received", request);

            ProductServiceFilterRequest? filterRequest = request?.ToProductServiceFilterRequest();
            var result = await _service.GetProducts(filterRequest, cancellationToken);

            _logger.LogInformation("Request processed", result?.Products?.Count);

            if (result == null || result.Products == null)
            {
                return NotFound(result);
            }

            return Ok(new ProductsDtoResponse()
            {
                Products = result.Products,
                Stat = result.Stat,
            });

        }

        //[HttpGet(Name = "filterBody")]
        //[Route("get")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[Produces(typeof(ProductsDto))]
        //public async Task<ActionResult<ProductsDto>> Get([FromBody] GetProductsRequest? request)
        //{
        //    _logger.LogInformation("Request received", request);

        //    ProductServiceFilterRequest? filterRequest = request?.ToProductServiceFilterRequest();
        //    var result = await _service.GetProducts(filterRequest);

        //    _logger.LogInformation("Request processed", result?.Products?.Count);

        //    if (result == null || result.Products?.Count == 0)
        //    {
        //        return NotFound(result);
        //    }
        //    return Ok(result);
        //}
    }
}