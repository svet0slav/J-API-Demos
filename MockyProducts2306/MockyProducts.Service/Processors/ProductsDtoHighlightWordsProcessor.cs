using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public class ProductsDtoHighlightWordsProcessor : IProductsDtoHighlightWordsProcessor
    {
        private readonly List<string>? _words;

        public ProductsDtoHighlightWordsProcessor(List<string>? words)
        {
            _words = words?.Count != 0 ? words : null;
        }

        public void Process(ProductDto product)
        {
            if (_words == null) return;

            product.Description = HighlightWords(product.Description);
        }

        public string? HighlightWords(string? text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var newText = text;
            _words?.ForEach(word =>
                {
                    if (newText?.Contains(word) ?? false)
                    { newText = newText?.Replace(word, "<em>" + word + "</em>"); }
                });

            return newText;
        }
    }
}
