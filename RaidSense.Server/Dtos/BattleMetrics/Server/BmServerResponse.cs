namespace RaidSense.Server.Dtos.BattleMetrics.Server
{
    public class BmServerResponse
    {
        public BmServerData Data { get; set; } = default!;
        public List<object>? Included { get; set; }
    }
}
