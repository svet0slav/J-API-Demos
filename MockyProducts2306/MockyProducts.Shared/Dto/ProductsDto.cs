using System.Text.Json.Serialization;

namespace MockyProducts.Shared.Dto
{
    public class ProductsDto
    {
        [JsonPropertyName("products")]
        public List<ProductDto>? Products { get; set;}

        [JsonPropertyName("total")]
        public ProductStatDto? Stat { get; set;}
    }
}
