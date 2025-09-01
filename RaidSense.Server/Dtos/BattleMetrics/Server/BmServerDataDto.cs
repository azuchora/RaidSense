namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerDataDto
    {
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public BmServerAttributesDto Attributes { get; set; } = null!;
    }
}
