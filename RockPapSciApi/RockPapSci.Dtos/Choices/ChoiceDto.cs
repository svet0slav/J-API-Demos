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

        /// <summary>
        /// Show easier in debugging.
        /// </summary>
        /// <returns>Format {Letter} ({Id}) {Name}, like R (1) Rock</returns>
        public override string ToString()
        {
            return $"{Name?.Substring(0,1)} ({Id}) {Name}";
        }
    }
}
