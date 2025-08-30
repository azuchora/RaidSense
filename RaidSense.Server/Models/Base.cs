namespace RaidSense.Server.Models
{
    public class Base
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public UserMap Map { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public ICollection<BasePlayer> BasePlayers { get; set; } = new List<BasePlayer>();
    }
}
