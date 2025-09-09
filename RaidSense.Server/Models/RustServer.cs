using System.ComponentModel.DataAnnotations.Schema;

namespace RaidSense.Server.Models
{
    [Table("RustServers")]
    public class RustServer
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime LastFetched { get; set; }
        public string? MapId { get; set; }
        public RustMap? Map { get; set; }
    }
}
