using System.Text.Json.Serialization;

namespace RaidSense.Server.Dtos.RustMaps
{
    public class RustMapsResponseDto
    {
        [JsonPropertyName("data")]
        public RustMapsDataDto? Data { get; set; }
    }
}
