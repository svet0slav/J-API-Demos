using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public interface IProductsDtoProcessor
    {
        List<string>? Words { get; set; }
        void Process(ProductDto product);
    }
}
