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
        public Map Map { get; set; } = null!;
        public ICollection<MapUser> MapUsers { get; set; } = new List<MapUser>();
        public ICollection<Base> Bases { get; set; } = new List<Base>();
    }
}
