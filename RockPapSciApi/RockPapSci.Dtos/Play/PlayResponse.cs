using System.Text.Json.Serialization;

namespace RockPapSci.Dtos.Play
{
    /// <summary>
    /// Response when Play a round against a bot..
    /// </summary>
    public class PlayResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("player")]
        public int PlayerChoice { get; set; }

        [JsonPropertyName("bot")]
        public int BotChoice { get; set; }
    }
}
