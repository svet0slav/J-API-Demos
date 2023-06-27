using MockyProducts.Repository.Data;

namespace MockyProducts.Service.Filters
{
    public interface IProductServiceFilter
    {
        IEnumerable<Product> Filter(IEnumerable<Product> filteredData);
        IEnumerable<Product> FilterByPrice(IEnumerable<Product> filteredData);
        IEnumerable<Product> FilterBySize(IEnumerable<Product> filteredData);
    }
}