using Microsoft.Extensions.Logging;
using MockyProducts.Repository.Data;
using MockyProducts.Repository.Readers;
using MockyProducts.Repository.Requests;
using MockyProducts.Service.Filters;
using MockyProducts.Service.Mappers;
using MockyProducts.Service.Processors;
using MockyProducts.Shared.Data;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;
using MockyProducts.Shared.Services;

namespace MockyProducts.Service
{
    public class MockyProductsService : IMockyProductsService
    {
        protected IMockyJsonReader _reader;
        protected IProductServiceFilter _filter;
        protected IProductsHighlightWordsProcessor _highlighter;
        protected IProductsStatsProcessor _statProcessor;
        protected ILogger<MockyProductsService> _logger;

        public MockyProductsService(IMockyJsonReader reader, IProductServiceFilter filter, 
            IProductsHighlightWordsProcessor highlighter,
            IProductsStatsProcessor statProcessor,
            ILogger<MockyProductsService> logger)
        {
            _reader = reader;
            _filter = filter;
            _highlighter = highlighter;
            _statProcessor = statProcessor;
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

            var highlightTask = new Task(() => Highlight(result?.Products, filterRequest), cancellationToken);
            var productData = (IEnumerable<Product>?)rawData?.Products;
            var statsTask = GetProductsStat(productData, cancellationToken);
            await Task.WhenAll(highlightTask, statsTask);
            result.Stat = statsTask.Result;
            return result;
        }

        protected void Highlight(List<ProductDto>? products, ProductServiceFilterRequest? filterRequest)
        {
            if (_highlighter != null && products != null)
            {
                _highlighter.Words = filterRequest.Highlight;
                foreach (var product in products)
                {
                    _highlighter.Process(product);
                }
            }
            _logger.LogInformation("Products highlighted");
        }

        public async Task<ProductStatDto?> GetProductsStat(IEnumerable<Product>? products, CancellationToken cancellationToken)
        {
            if (_statProcessor == null)
                throw new NullReferenceException("The stat processor is not available.");
            _logger.LogInformation("Statistics computation started");
            var statDto = await _statProcessor.Summarize(products, cancellationToken);

            return statDto;
        }
    }
}