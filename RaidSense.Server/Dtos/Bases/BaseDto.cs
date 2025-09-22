using RaidSense.Server.Models;

namespace RaidSense.Server.Dtos.Bases
{
    public class BaseDto
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
    }
}
