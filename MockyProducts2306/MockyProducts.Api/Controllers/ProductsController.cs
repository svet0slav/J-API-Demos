using Microsoft.AspNetCore.Mvc;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.Services;
using MockyProducts.Shared.ServiceRequests;
using MockyProducts.Shared.Requests;
using MockyProducts.Shared.ServiceRequests.Mappers;

namespace MockyProducts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMockyProductsService _service;

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMockyProductsService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet(Name = "filter")]
        [Produces(typeof(ProductsDto))]
        public async Task<ProductsDto> Get(GetProductsRequest? request)
        {
            _logger.LogInformation("Request received", request);

            ProductServiceFilterRequest? filterRequest = request?.ToProductServiceFilterRequest();
            var result = await _service.GetProducts(filterRequest);
            
            _logger.LogInformation("Request processed", result?.Products?.Count);
            
            return result ?? new ProductsDto();
        }
    }
}