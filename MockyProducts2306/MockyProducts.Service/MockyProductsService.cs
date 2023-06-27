using MockyProducts.Repository.Data;
using MockyProducts.Repository.Readers;
using MockyProducts.Repository.Requests;
using MockyProducts.Service.Filters;
using MockyProducts.Service.Mappers;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;
using MockyProducts.Shared.Services;

namespace MockyProducts.Service
{
    public class MockyProductsService : IMockyProductsService
    {
        protected IMockyJsonReader _reader;
        protected IProductServiceFilter _filter;

        public MockyProductsService(IMockyJsonReader reader, IProductServiceFilter filter)
        {
            _reader = reader;
            _filter = filter;
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



            return result;
        }

        public ProductsDto HighlightWords(ProductsDto? data, ProductServiceFilterRequest? filterRequest)
        {
            if (data == null) throw new ArgumentNullException("data");

            if (filterRequest == null || filterRequest.Highlight?.Count == 0)
                return data;

            List<string> highlight = filterRequest.Highlight ?? new List<string>();

            // TODO: In case of large list, consider async processing.
            data.Products?.ForEach(product =>
                {
                    highlight.ForEach(word =>
                        {
                            if (product.Description?.Contains(word) ?? false)
                            { product.Description = product.Description?.Replace(word, "<em>" + highlight + "</em>"); }
                        });
                });

            return data;
        }
    }
}