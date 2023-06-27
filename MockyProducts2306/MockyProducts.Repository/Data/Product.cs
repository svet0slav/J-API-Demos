using System.Text.Json.Serialization;

namespace MockyProducts.Repository.Data
{
    public class Product
    {
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("sizes")]
        public List<string>? Sizes { get; set; }
    }
}
