namespace RaidSense.Server.Dtos.RustMaps
{
    public class RustMapDto
    {
        public string Id { get; set; } = string.Empty;
        public int Seed { get; set; }
        public int Size { get; set; }
        public bool IsCustomMap { get; set; }
        public bool IsStaging { get; set; }
        public string RawImageUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Url {  get; set; } = string.Empty;
        public bool IsPopulated { get; set; }
    }
}
