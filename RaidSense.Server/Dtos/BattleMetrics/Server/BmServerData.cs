namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerData
    {
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public BmServerAttributes Attributes { get; set; } = null!;
    }
}
