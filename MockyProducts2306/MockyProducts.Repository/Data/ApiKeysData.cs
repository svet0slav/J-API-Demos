using System.Text.Json.Serialization;

namespace MockyProducts.Repository.Data
{
    public class ApiKeysData
    {
        [JsonPropertyName("primary")]
        public string? Primary { get; set; }

        [JsonPropertyName("secondary")]
        public string? Secondary { get; set; }
    }
}
