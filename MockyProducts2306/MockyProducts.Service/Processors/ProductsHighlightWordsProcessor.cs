using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public class ProductsHighlightWordsProcessor : IProductsHighlightWordsProcessor
    {
        public List<string>? Words { get; set; }

        public ProductsHighlightWordsProcessor() { }

        public void Process(ProductDto product)
        {
            if (Words == null || Words.Count == 0) return;

            product.Description = HighlightWords(product.Description);
        }

        public string? HighlightWords(string? text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var newText = text;
            Words?.ForEach(word =>
                {
                    if (newText?.Contains(word) ?? false)
                    { newText = newText?.Replace(word, "<em>" + word + "</em>"); }
                });

            return newText;
        }
    }
}
