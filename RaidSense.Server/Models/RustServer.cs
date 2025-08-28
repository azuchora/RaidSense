namespace RaidSense.Server.Models
{
    public class RustServer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MapId { get; set; } = string.Empty;
        public DateTime LastFetched { get; set; }
        public ICollection<Map> Maps { get; set; } = new List<Map>();
    }
}
