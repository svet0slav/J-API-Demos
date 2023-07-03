using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public interface IProductsHighlightWordsProcessor: IProductsDtoProcessor
    {
        string? HighlightWords(string? text);
    }
}