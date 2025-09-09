using RaidSense.Server.Models;

namespace RaidSense.Server.Dtos.UserMap
{
    public class UserMapDto
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public RustMap? Map { get; set; }
        public List<MapUser>? MapUsers { get; set; }
        public List<Base>? Bases { get; set; }
    }
}
