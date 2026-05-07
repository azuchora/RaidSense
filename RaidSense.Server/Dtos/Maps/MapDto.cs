using RaidSense.Server.Dtos.RustMaps;

namespace RaidSense.Server.Dtos.Maps
{
    public class MapDto
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public RustMapDto? Map { get; set; }
        public List<MapUserDto>? MapUsers { get; set; }
    }
}
