using MockyProducts.Shared.Data;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.Shared.Services
{
    public interface IMockyProductsService
    {
        Task<ProductsDto?> GetProducts(ProductServiceFilterRequest? filterRequest, CancellationToken cancellationToken);
        Task<ProductStatDto?> GetProductsStat(IEnumerable<IProduct>? products, ProductServiceFilterRequest? filterRequest, CancellationToken cancellationToken);
    }
}
