namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerResponseDto
    {
        public BmServerDataDto Data { get; set; } = default!;
        public List<object>? Included { get; set; }
    }
}
