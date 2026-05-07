using System.ComponentModel.DataAnnotations;

namespace RaidSense.Server.Dtos.Maps
{
    public class UpdateMapDto
    {
        [Required]
        public string MapId { get; set; } = string.Empty;
    }
}
