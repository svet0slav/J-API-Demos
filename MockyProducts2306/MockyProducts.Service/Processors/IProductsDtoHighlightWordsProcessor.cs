using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public interface IProductsDtoHighlightWordsProcessor: IProductsDtoProcessor
    {
        string? HighlightWords(string? text);
    }
}