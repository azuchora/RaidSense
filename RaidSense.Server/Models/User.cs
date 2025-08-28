using Microsoft.AspNetCore.Identity;

namespace RaidSense.Server.Models
{
    public class User : IdentityUser
    {
        public ICollection<Map>? OwnedMaps {  get; set; }
        public ICollection<MapUser>? MapAccesses { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
