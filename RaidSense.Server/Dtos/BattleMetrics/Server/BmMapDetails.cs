namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmMapDetails
    {
        public int Seed { get; set; }
        public int Size { get; set; }
        public string Url { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string MapUrl { get; set; } = string.Empty;
    }
}
