using System.Text.Json.Serialization;

namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerDetails
    {
        [JsonPropertyName("rust_headerimage")]
        public string HeaderImage { get; set; } = string.Empty;

        [JsonPropertyName("rust_url")]
        public string RustUrl { get; set; } = string.Empty;

        [JsonPropertyName("rust_world_seed")]
        public int Seed { get; set; }

        [JsonPropertyName("rust_world_size")]
        public int Size { get; set; }

        [JsonPropertyName("rust_world_levelurl")]
        public string WorldLevelUrl { get; set; } = string.Empty;

        [JsonPropertyName("rust_maps")]
        public BmMapDetails RustMaps { get; set; } = new();

        [JsonPropertyName("rust_last_wipe")]
        public DateTime RustLastWipe { get; set; }

        [JsonPropertyName("rust_next_wipe")]
        public DateTime RustNextWipe { get; set; }

        [JsonPropertyName("rust_next_wipe_map")]
        public DateTime RustNextWipeMap { get; set; }

        [JsonPropertyName("serverSteamId")]
        public string ServerSteamId { get; set; } = string.Empty;
    }
}
