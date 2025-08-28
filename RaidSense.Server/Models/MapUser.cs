namespace RaidSense.Server.Models
{
    public class MapUser
    {
        public int Id { get; set; }
        public string MapId { get; set; } = null!;
        public Map Map { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public MapRole Role { get; set; }
    }

    public enum MapRole { Viewer, Editor, Admin }
}
