using MockyProducts.Repository.Data;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.Service.Filters
{
    public class ProductServiceFilter : IProductServiceFilter
    {
        protected readonly ProductServiceFilterRequest? _filterRequest;

        public ProductServiceFilter(ProductServiceFilterRequest? filterRequest)
        {
            _filterRequest = filterRequest;
        }

        public IEnumerable<Product> Filter(IEnumerable<Product> filteredData)
        {
            // Filter step by step.
            var result = FilterByPrice(filteredData);
            result = FilterBySize(result);
            return result;
        }

        public IEnumerable<Product> FilterByPrice(IEnumerable<Product> filteredData)
        {
            if (_filterRequest == null) return filteredData;
            if (_filterRequest.MinPrice.HasValue && _filterRequest.MaxPrice.HasValue)
            {
                filteredData = filteredData.Where(p => p.Price >= _filterRequest.MinPrice.Value && p.Price <= _filterRequest.MaxPrice.Value);
            }
            else if (_filterRequest.MinPrice.HasValue)
            {
                filteredData = filteredData.Where(p => p.Price >= _filterRequest.MinPrice.Value);
            }
            else if (_filterRequest.MaxPrice.HasValue)
            {
                filteredData = filteredData.Where(p => p.Price <= _filterRequest.MaxPrice.Value);
            }
            return filteredData;
        }

        public IEnumerable<Product> FilterBySize(IEnumerable<Product> filteredData)
        {
            if (_filterRequest == null) return filteredData;

            if (string.IsNullOrEmpty(_filterRequest.Size))
                return filteredData;

            filteredData = filteredData.Where(p => p.Sizes != null && p.Sizes.Contains(_filterRequest.Size));
            return filteredData;
        }
    }
}
