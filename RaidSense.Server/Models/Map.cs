namespace RaidSense.Server.Models
{
    public class Map
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ServerId { get; set; }
        public RustServer? Server { get; set; }
        public int Seed { get; set; }
        public int Size { get; set; }
        public bool isCustomMap { get; set; }
        public bool isStaging { get; set; }
        public string OwnerId { get; set; } = null!;
        public User Owner { get; set; } = null!;
        public ICollection<MapUser> MapUsers { get; set; } = new List<MapUser>();
        public ICollection<Base> Bases { get; set; } = new List<Base>();
    }
}
