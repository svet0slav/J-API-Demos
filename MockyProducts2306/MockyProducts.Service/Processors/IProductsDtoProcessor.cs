using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public interface IProductsDtoProcessor
    {
        void Process(ProductDto product);
    }
}
