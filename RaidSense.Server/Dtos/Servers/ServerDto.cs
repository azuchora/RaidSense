namespace RaidSense.Server.Dtos.Servers
{
    public class ServerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? MapId { get; set; } = string.Empty;
        public DateTime LastFetched { get; set; }
    }
}
