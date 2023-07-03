using MockyProducts.Repository.Data;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.Service.Filters
{
    public class ProductServiceFilter : IProductServiceFilter
    {
        public ProductServiceFilter()
        {
        }

        public IEnumerable<Product> Filter(IEnumerable<Product> filteredData, ProductServiceFilterRequest? filterRequest, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return  Enumerable.Empty<Product>(); }
            var result = FilterByPrice(filteredData, filterRequest);
            if (cancellationToken.IsCancellationRequested)
            {
                return result;
            }

            result = FilterBySize(result, filterRequest);
            return result;
        }

        public IEnumerable<Product> FilterByPrice(IEnumerable<Product> filteredData, ProductServiceFilterRequest? filterRequest)
        {
            if (filterRequest == null) return filteredData;
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
            return filteredData;
        }

        public IEnumerable<Product> FilterBySize(IEnumerable<Product> filteredData, ProductServiceFilterRequest? filterRequest)
        {
            if (filterRequest == null) return filteredData;

            if (string.IsNullOrEmpty(filterRequest.Size))
                return filteredData;

            filteredData = filteredData.Where(p => p.Sizes != null && p.Sizes.Contains(filterRequest.Size));
            return filteredData;
        }
    }
}
