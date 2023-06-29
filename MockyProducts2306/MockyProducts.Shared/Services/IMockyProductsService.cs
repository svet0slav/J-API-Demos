using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.Shared.Services
{
    public interface IMockyProductsService
    {
        Task<ProductsDto?> GetProducts(ProductServiceFilterRequest? filterRequest);
    }
}