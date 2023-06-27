using System.Text.Json.Serialization;

namespace MockyProducts.Repository.Data
{
    public class ProductsSource
    {
        [JsonPropertyName("products")]
        public List<Product>? Products { get; set; }

        [JsonPropertyName("apiKeys")]
        public ApiKeysData ApiKeys { get; set; }
        
    }
}
