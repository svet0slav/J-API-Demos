using Microsoft.Extensions.Logging;
using MockyProducts.Repository.Data;
using MockyProducts.Repository.Readers;
using MockyProducts.Repository.Requests;
using MockyProducts.Service.Filters;
using MockyProducts.Service.Mappers;
using MockyProducts.Service.Processors;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;
using MockyProducts.Shared.Services;

namespace MockyProducts.Service
{
    public class MockyProductsService : IMockyProductsService
    {
        protected IMockyJsonReader _reader;
        protected IProductServiceFilter _filter;
        protected IProductsDtoProcessor _processor;
        protected ILogger<MockyProductsService> _logger;

        public MockyProductsService(IMockyJsonReader reader, IProductServiceFilter filter, IProductsDtoProcessor processor,
            ILogger<MockyProductsService> logger)
        {
            _reader = reader;
            _filter = filter;
            _processor = processor;
            _logger = logger;
        }

        public async Task<ProductsDto?> GetProducts(ProductServiceFilterRequest? filterRequest)
        {
            if (filterRequest == null)
                throw new ArgumentNullException(nameof(filterRequest));

            MockyRawDataParams param = new MockyRawDataParams();
            // TODO: Setup additional params from the filter
            var rawData = await _reader.GetRawDataFromSource(param);

            if (rawData == null || rawData.Products?.Count == 0)
            {
                _logger.LogInformation("No records returned");
                return new ProductsDto();
            }

            _logger.LogInformation($"{rawData?.Products?.Count} records returned");

            var result = new ProductsDto();
            result.Products = new List<ProductDto>();

            if (rawData?.Products == null) return result;

            IEnumerable<Product> filteredData = rawData.Products;

            filteredData = _filter.Filter(filteredData, filterRequest);
            
            result.Products.AddRange(filteredData.Select(product => product.ConvertToDto()));

            _logger.LogInformation($"{result?.Products?.Count} records returned after filtering.");

            if (_processor != null && result?.Products != null)
            {
                foreach (var product in result.Products)
                {
                    _processor.Process(product);
                }
            }

            return result;
        }
    }
}