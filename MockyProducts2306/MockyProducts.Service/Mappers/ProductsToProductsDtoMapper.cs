using MockyProducts.Repository.Data;
using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Mappers
{
    public static class ProductsToProductsDtoMapper
    {
        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Sizes = product.Sizes,
            };
        }
    }
}
