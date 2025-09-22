using RaidSense.Server.Dtos.Bases;
using RaidSense.Server.Dtos.Map;
using RaidSense.Server.Dtos.MapUsers;

namespace RaidSense.Server.Dtos.UserMap
{
    public class UserMapDto
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public MapDto? Map { get; set; }
        public List<MapUserDto>? MapUsers { get; set; }
        public List<BaseDto>? Bases { get; set; }
    }
}
