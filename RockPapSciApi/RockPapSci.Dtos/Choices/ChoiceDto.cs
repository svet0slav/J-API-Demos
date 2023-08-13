using System.Text.Json.Serialization;

namespace RockPapSci.Dtos.Choices
{
    /// <summary>
    /// The choice visible to the api client.
    /// </summary>
    public class ChoiceDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
