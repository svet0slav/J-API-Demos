using MockyProducts.Repository.Data;
using MockyProducts.Repository.Readers;
using MockyProducts.Repository.Requests;
using MockyProducts.Service.Mappers;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.Service
{
    public class MockyProductsService
    {
        protected IMockyJsonReader _reader;

        public MockyProductsService(IMockyJsonReader reader) {
            _reader = reader;
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

            if (!filterRequest.MinPrice.HasValue && !filterRequest.MaxPrice.HasValue
                && string.IsNullOrEmpty(filterRequest.Size)
                && filterRequest.Highlight?.Count == 0)
            {
                // Yes, leave all.
            } else
            {
                // Filter step by step.
                if (filterRequest.MinPrice.HasValue && filterRequest.MaxPrice.HasValue)
                {
                    filteredData = filteredData.Where(p => p.Price >= filterRequest.MinPrice.Value && p.Price <= filterRequest.MaxPrice.Value);
                }
                else if (filterRequest.MinPrice.HasValue)
                {
                    filteredData = filteredData.Where(p => p.Price >= filterRequest.MinPrice.Value);
                }
                else if (filterRequest.MaxPrice.HasValue)
                {
                    filteredData = filteredData.Where(p => p.Price <= filterRequest.MaxPrice.Value);
                }

                if (!string.IsNullOrEmpty(filterRequest.Size))
                {
                    filteredData = filteredData.Where(p => p.Sizes != null && p.Sizes.Contains(filterRequest.Size));
                }
            }

            result.Products.AddRange(filteredData.Select(product => product.ConvertToDto()));

            return result;
        }

        public ProductsDto HighlightWords(ProductsDto data, ProductServiceFilterRequest? filterRequest)
        {
            if (filterRequest == null || filterRequest.Highlight?.Count == 0)
                return data;

            List<string> highlight = filterRequest.Highlight ?? new List<string>();

            data.Products.ForEach(product =>
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