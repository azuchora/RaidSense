using System.Text.Json.Serialization;

namespace RaidSense.Server.Dtos.RustMaps
{
    public class RustMapsDataDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("rawImageUrl")]
        public string RawImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("thumbnailUrl")]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [JsonPropertyName("isStaging")]
        public bool IsStaging { get; set; }

        [JsonPropertyName("isCustomMap")]
        public bool IsCustomMap { get; set; }
    }
}
