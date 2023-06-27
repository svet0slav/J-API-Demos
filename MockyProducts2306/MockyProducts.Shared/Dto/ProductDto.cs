using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MockyProducts.Shared.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title {  get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("sizes")]
        public List<string>? Sizes { get; set; }
    }
}
