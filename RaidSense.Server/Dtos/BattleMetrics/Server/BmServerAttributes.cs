namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerAttributes
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }
        public BmServerDetails Details { get; set; } = null!;
    }
}
