using RaidSense.Server.Dtos.Maps;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class MapUserMapper
    {
        public static MapUserDto ToDto(this MapUser user)
        {
            return new MapUserDto
            {
                UserId = user.UserId,
                Username = user.User?.UserName ?? string.Empty,
                Role = user!.Role,
            };
        }
    }
}
