using System.Text.Json.Serialization;

namespace RockPapSci.Service.Https
{
    public class RandomResponseObject
    {
        [JsonPropertyName("random")]
        public int? Random { get; set; }
    }
}
