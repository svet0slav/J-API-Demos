using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public interface IProductsHighlightWordsProcessor: IProductsDtoProcessor
    {
        List<string>? Words { get; set; }
        string? HighlightWords(string? text);
    }
}