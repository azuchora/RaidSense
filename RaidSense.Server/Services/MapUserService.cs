using RaidSense.Server.Exceptions.Http;
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

        public async Task GrantAccessAsync(string invokerId, string userId, int userMapId)
        {
            EnsureNotInvoker(invokerId, userId);
            var existing = await GetMapUserAsync(userId, userMapId);

            if (existing is not null)
                throw new ConflictException("User already has access");

            var newMapUser = new MapUser
            {
                UserId = userId,
                MapId = userMapId,
                Role = MapRole.Viewer
            };

            await _mapUserRepo.AddAndSaveAsync(newMapUser);
        }

        public async Task RevokeAccessAsync(string invokerId, string userId, int userMapId)
        {
            EnsureNotInvoker(invokerId, userId);
            var mapUser = await GetMapUserOrThrowAsync(userId, userMapId);

            await ValidateRoleRevokeAsync(invokerId, mapUser, userMapId);

            await _mapUserRepo.DeleteAsync(mapUser);
        }

        public async Task UpdateRoleAsync(string invokerId, string userId, int userMapId, MapRole newRole)
        {
            EnsureNotInvoker(invokerId, userId);
            var mapUser = await GetMapUserOrThrowAsync(userId, userMapId);

            await ValidateRoleUpdateAsync(invokerId, mapUser, userMapId, newRole);

            mapUser.Role = newRole;

            await _mapUserRepo.UpdateAsync(mapUser);
        }

        public async Task<bool> HasRoleAsync(string userId, int mapId, MapRole minimumRole)
        {
            var mapUser = await GetMapUserAsync(userId, mapId);

            return mapUser?.Role >= minimumRole;
        }

        public async Task AssignOwnerAsync(string userId, int mapId)
        {
            var existing = await GetMapUserAsync(userId, mapId);
            if (existing is not null)
                throw new ConflictException("User already has access");

            var newMapUser = new MapUser
            {
                UserId = userId,
                MapId = mapId,
                Role = MapRole.Owner
            };

            await _mapUserRepo.AddAndSaveAsync(newMapUser);
        }

        private async Task<MapUser?> GetMapUserAsync(string userId, int mapId)
        {
            return await _mapUserRepo.GetMapUserAsync(userId, mapId);
        }

        private async Task<MapUser> GetMapUserOrThrowAsync(string userId, int mapId)
        {
            var user = await GetMapUserAsync(userId, mapId);
            return user ?? throw new NotFoundException("Mapuser not found");
        }

        private void EnsureNotInvoker(string invokerId, string userId)
        {
            if(invokerId == userId) 
                throw new ForbiddenException("You cannot modify your own roles");
        }

        private async Task ValidateRoleUpdateAsync(string invokerId, MapUser target, int mapId, MapRole newRole)
        {
            if(target.Role == MapRole.Owner)
                throw new ForbiddenException("Cannot modify owner roles");

            if (newRole == MapRole.Owner) 
                throw new ForbiddenException("Cannot assign Owner role");

            var invoker = await GetMapUserOrThrowAsync(invokerId, mapId);
            
            if(newRole >= invoker.Role)
                throw new ForbiddenException("Cannot assign higher role than your own");

            if(invoker.Role <= target.Role)
                throw new ForbiddenException("Cannot modify user with higher role");   
        }

        private async Task ValidateRoleRevokeAsync(string invokerId, MapUser target, int mapId)
        {
            if (target.Role == MapRole.Owner)
                throw new ForbiddenException("Cannot revoke owner");

            var invoker = await GetMapUserOrThrowAsync(invokerId, mapId);

            if (invoker.Role <= target.Role)
            {
               throw new ForbiddenException("Cannot revoke user with equal or higher role"); 
            }
        }
    }
}
