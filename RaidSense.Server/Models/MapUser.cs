namespace RaidSense.Server.Models
{
    public class MapUser
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public UserMap Map { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public MapRole Role { get; set; } = MapRole.Viewer;
    }

    public enum MapRole { Viewer, Editor, Admin, Owner }
}
