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
using System.Threading.Tasks;

namespace MockyProducts.Service
{
    public class MockyProductsService : IMockyProductsService
    {
        protected IMockyJsonReader _reader;
        protected IProductServiceFilter _filter;
        protected IProductsHighlightWordsProcessor _highlighter;
        protected ILogger<MockyProductsService> _logger;

        public MockyProductsService(IMockyJsonReader reader, IProductServiceFilter filter, 
            IProductsHighlightWordsProcessor highlighter,
            ILogger<MockyProductsService> logger)
        {
            _reader = reader;
            _filter = filter;
            _highlighter = highlighter;
            _logger = logger;
        }

        public async Task<ProductsDto?> GetProducts(ProductServiceFilterRequest? filterRequest, CancellationToken cancellationToken)
        {
            if (filterRequest == null)
                throw new ArgumentNullException(nameof(filterRequest));

            MockyRawDataParams param = new MockyRawDataParams();
            // TODO: Setup additional params from the filter
            var rawData = await _reader.GetRawDataFromSource(param, cancellationToken);

            if (rawData == null || rawData?.Products == null)
            {
                _logger.LogInformation("No records returned");
                return new ProductsDto();
            }

            _logger.LogInformation($"{rawData?.Products?.Count} records returned");

            var result = new ProductsDto();
            result.Products = new List<ProductDto>();

            IEnumerable<Product> filteredData = rawData?.Products;

            filteredData = _filter.Filter(filteredData, filterRequest, cancellationToken);
            
            result.Products.AddRange(filteredData.Select(product => product.ConvertToDto()));

            _logger.LogInformation($"{result?.Products?.Count} records returned after filtering.");

            if (_highlighter != null && result?.Products != null)
            {
                _highlighter.Words = filterRequest.Highlight;
                foreach (var product in result.Products)
                {
                    _highlighter.Process(product);
                }
            }
            return result;
        }
    }
}