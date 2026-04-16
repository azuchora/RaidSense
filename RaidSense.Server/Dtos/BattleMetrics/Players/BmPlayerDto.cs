namespace RaidSense.Server.Dtos.BattleMetrics.Players;

public class BmPlayerDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool Private { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
