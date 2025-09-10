using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class MapUserService : IMapUserService
    {
        private readonly IMapUserRepository _mapUserRepo;
        public MapUserService(IMapUserRepository mapUserRepo)
        {
            _mapUserRepo = mapUserRepo;
        }

        private async Task<MapUser?> GetMapUserAsync(string userId, int mapId)
        {
            return await _mapUserRepo.GetQueryable()
                .FirstOrDefaultAsync(mu => mu.UserId == userId && mu.MapId == mapId);
        }

        public async Task<bool> GrantAccessAsync(string userId, int userMapId, MapRole role)
        {
            var existing = await GetMapUserAsync (userId, userMapId);

            if (existing != null)
                return false;

            var newMapUser = new MapUser
            {
                UserId = userId,
                MapId = userMapId,
                Role = role
            };

            await _mapUserRepo.AddAndSaveAsync(newMapUser);
            return true;
        }

        public async Task<bool> RevokeAccessAsync(string userId, int userMapId)
        {
            var mapUser = await GetMapUserAsync(userId, userMapId);

            if (mapUser == null)
                return false;

            await _mapUserRepo.DeleteAsync(mapUser);
            return true;
        }

        public async Task<bool> UpdateRoleAsync(string userId, int userMapId, MapRole newRole)
        {
            var mapUser = await GetMapUserAsync(userId, userMapId);

            if (mapUser == null)
                return false;

            mapUser.Role = newRole;
            await _mapUserRepo.UpdateAsync(mapUser);
            return true;
        }

        public async Task<bool> HasRoleAsync(string userId, int mapId, MapRole minimumRole)
        {
            var mapUser = await GetMapUserAsync(userId, mapId);

            if (mapUser == null)
                return false;

            return mapUser.Role >= minimumRole;
        }
    }
}
