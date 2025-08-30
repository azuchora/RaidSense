namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }
        public int? Seed { get; set; }
        public int? Size { get; set; }
        public string? Url { get; set; } = string.Empty;
        public string? MapId { get; set; } = string.Empty;
    }
}
