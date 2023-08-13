using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RockPapSci.Dtos.Play
{
    /// <summary>
    /// Play a round against a bot. This is the choice of the player.
    /// </summary>
    public class PlayRequest
    {
        [JsonPropertyName("player")]
        [JsonInclude()]
        [Required]
        public string PlayerChoice { get; set; }
    }
}
