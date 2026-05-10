using RaidSense.Server.Exceptions.Http;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class MapAccessService : IMapAccessService
    {
        private readonly IMapAccessRepository _mapAccessRepo;
        public MapAccessService(IMapAccessRepository mapAccessRepo)
        {
            _mapAccessRepo = mapAccessRepo;
        }

        public Task EnsureViewerAsync(string userId, int mapId) =>
            EnsureHasRoleAsync(userId, mapId, MapRole.Viewer);

        public Task EnsureEditorAsync(string userId, int mapId) =>
            EnsureHasRoleAsync(userId, mapId, MapRole.Editor);

        public Task EnsureAdminAsync(string userId, int mapId) =>
            EnsureHasRoleAsync(userId, mapId, MapRole.Admin);

        public Task EnsureOwnerAsync(string userId, int mapId) =>
            EnsureHasRoleAsync(userId, mapId, MapRole.Owner);

        public async Task EnsureHasRoleAsync(string userId, int mapId, MapRole requiredRole)
        {
            var user = await GetMapUserAsync(userId, mapId);
            if (user is null)
                throw new ForbiddenException("No access to this map");

            if (user.Role < requiredRole)
                throw new ForbiddenException("Insufficient permission");
        }

        public async Task GrantAccessAsync(string invokerId, string userId, int userMapId)
        {
            EnsureNotInvoker(invokerId, userId);

            var invoker = await GetMapUserOrThrowAsync(invokerId, userMapId);
            EnsureAdmin(invoker);

            var existing = await GetMapUserAsync(userId, userMapId);

            if (existing is not null)
                throw new ConflictException("User already has access");

            var newMapUser = new MapUser
            {
                UserId = userId,
                MapId = userMapId,
                Role = MapRole.Viewer
            };

            await _mapAccessRepo.AddAndSaveAsync(newMapUser);
        }

        public async Task RevokeAccessAsync(string invokerId, string userId, int userMapId)
        {
            EnsureNotInvoker(invokerId, userId);

            var invoker = await GetMapUserOrThrowAsync(invokerId, userMapId);
            EnsureAdmin(invoker);

            var target = await GetMapUserAsync(userId, userMapId);

            if (target is null) return;

            if (target.Role == MapRole.Owner)
                throw new ForbiddenException("Cannot revoke owner");

            if (invoker.Role <= target.Role)
                throw new ForbiddenException("Cannot revoke user with equal or higher role");

            await _mapAccessRepo.DeleteAsync(target);
        }

        public async Task UpdateRoleAsync(string invokerId, string userId, int userMapId, MapRole newRole)
        {
            EnsureNotInvoker(invokerId, userId);

            var invoker = await GetMapUserOrThrowAsync(invokerId, userMapId);
            EnsureAdmin(invoker);

            var target = await GetMapUserOrThrowAsync(userId, userMapId);

            if (target.Role == MapRole.Owner)
                throw new ForbiddenException("Cannot modify owner roles");

            if (newRole == MapRole.Owner)
                throw new ForbiddenException("Cannot assign Owner role");

            if (newRole >= invoker.Role)
                throw new ForbiddenException("Cannot assign higher role or equal than your own");

            if (invoker.Role <= target.Role)
                throw new ForbiddenException("Cannot modify user with higher or equal role");

            target.Role = newRole;

            await _mapAccessRepo.UpdateAsync(target);
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

            await _mapAccessRepo.AddAndSaveAsync(newMapUser);
        }

        public async Task<List<int>> GetAccessibleMapIdsAsync(string userId, MapRole minimumRole)
        {
            return await _mapAccessRepo.GetAccessibleMapIdsAsync(userId, minimumRole);
        }

        private async Task<MapUser?> GetMapUserAsync(string userId, int mapId)
        {
            return await _mapAccessRepo.GetMapUserAsync(userId, mapId);
        }

        private async Task<MapUser> GetMapUserOrThrowAsync(string userId, int mapId)
        {
            var user = await GetMapUserAsync(userId, mapId);
            return user ?? throw new ForbiddenException("No access to this map");
        }

        private void EnsureNotInvoker(string invokerId, string userId)
        {
            if(invokerId == userId) 
                throw new ForbiddenException("You cannot modify your own roles");
        }

        private void EnsureAdmin(MapUser invoker)
        {
            if(invoker.Role < MapRole.Admin)
                throw new ForbiddenException("Insufficient permission");
        }
    }
}
