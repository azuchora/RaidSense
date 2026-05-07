using RaidSense.Server.Dtos.Maps;
using RaidSense.Server.Models;

namespace RaidSense.Server.Mappers
{
    public static class UserMapsMapper
    {
        public static MapDto ToDto(this UserMap userMap)
        {
            return new MapDto
            {
                Id = userMap.Id,
                OwnerId = userMap.OwnerId,
                Map = userMap.Map?.ToDto(),
                MapUsers = userMap.MapUsers.Select(mu => mu.ToDto()).ToList(),
            };
        }

        public static UserMap ToEntity(this CreateMapDto dto, string ownerId)
        {
            return new UserMap
            {
                MapId = dto.MapId,
                OwnerId = ownerId,
            };
        }
    }
}
