using RaidSense.Server.Models;

namespace RaidSense.Server.Dtos.MapUsers
{
    public class MapUserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public MapRole Role { get; set; }
    }
}
