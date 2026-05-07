using System.ComponentModel.DataAnnotations.Schema;

namespace RaidSense.Server.Models
{
    [Table("UserMaps")]
    public class UserMap
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = null!;
        public User Owner { get; set; } = null!;
        public string MapId { get; set; } = null!;
        public RustMap Map { get; set; } = null!;
        public ICollection<MapUser> MapUsers { get; set; } = new List<MapUser>();
    }
}
