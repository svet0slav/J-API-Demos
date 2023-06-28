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

        public MockyProductsService(IMockyJsonReader reader, IProductServiceFilter filter, IProductsDtoProcessor processor)
        {
            _reader = reader;
            _filter = filter;
            _processor = processor;
        }

        public async Task<ProductsDto> GetProducts(ProductServiceFilterRequest? filterRequest)
        {
            if (filterRequest == null)
                throw new ArgumentNullException(nameof(filterRequest));

            MockyRawDataParams param = new MockyRawDataParams();
            // TODO: Setup additional params from the filter
            var rawData = await _reader.GetRawDataFromSource(param);

            if (rawData == null || rawData.Products?.Count == 0)
                return new ProductsDto();

            var result = new ProductsDto();
            result.Products = new List<ProductDto>();

            if (rawData?.Products == null) return result;

            IEnumerable<Product> filteredData = rawData.Products;

            filteredData = _filter.Filter(filteredData);
            
            result.Products.AddRange(filteredData.Select(product => product.ConvertToDto()));

            if (_processor != null && result.Products != null)
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