using MockyProducts.Repository.Data;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.Service.Filters
{
    public interface IProductServiceFilter
    {
        IEnumerable<Product> Filter(IEnumerable<Product> filteredData, ProductServiceFilterRequest? filterRequest, CancellationToken cancellationToken);
        IEnumerable<Product> FilterByPrice(IEnumerable<Product> filteredData, ProductServiceFilterRequest? filterRequest);
        IEnumerable<Product> FilterBySize(IEnumerable<Product> filteredData, ProductServiceFilterRequest? filterRequest);
    }
}