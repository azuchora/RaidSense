namespace RaidSense.Server.Models
{
    public class Map
    {
        public string Id { get; set; } = string.Empty;
        public int Seed { get; set; }
        public int Size { get; set; }
        public bool isCustomMap { get; set; }
        public bool isStaging { get; set; }
        public ICollection<RustServer> Servers { get; set; } = new List<RustServer>();
        public ICollection<UserMap> UserMaps { get; set; } = new List<UserMap>();
    }
}
