namespace RaidSense.Server.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? SteamId { get; set; }
        public string? BattlemetricsId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<BasePlayer> BasePlayers { get; set; } = new List<BasePlayer>();
    }
}
