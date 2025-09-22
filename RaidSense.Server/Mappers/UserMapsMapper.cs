using RaidSense.Server.Dtos.UserMap;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class UserMapsMapper
    {
        public static UserMapDto ToDto(this UserMap userMap)
        {
            return new UserMapDto
            {
                Id = userMap.Id,
                OwnerId = userMap.OwnerId,
                Map = userMap.Map?.ToDto(),
                MapUsers = userMap.MapUsers.Select(mu => mu.ToDto()).ToList(),
                Bases = userMap.Bases.Select(b => b.ToDto()).ToList(),
            };
        }

        public static UserMap ToEntity(this CreateUserMapDto dto, string ownerId)
        {
            return new UserMap
            {
                MapId = dto.MapId,
                OwnerId = ownerId,
            };
        }
    }
}
