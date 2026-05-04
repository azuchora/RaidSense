using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapAccessService
    {
        Task GrantAccessAsync(string invokerId, string userId, int userMapId);
        Task RevokeAccessAsync(string invokerId, string userId, int userMapId);
        Task UpdateRoleAsync(string invokerId, string userId, int userMapId, MapRole newRole);
        Task<bool> HasRoleAsync(string userId, int mapId, MapRole minimumRole);
        Task EnsureHasRoleAsync(string userId, int mapId, MapRole requiredRole);
        Task EnsureViewerAsync(string userId, int mapId);
        Task EnsureEditorAsync(string userId, int mapId);
        Task EnsureAdminAsync(string userId, int mapId);
        Task EnsureOwnerAsync(string userId, int mapId);
        Task AssignOwnerAsync(string userId, int mapId);
        Task<List<int>> GetAccessibleMapIdsAsync(string userId, MapRole minimumRole);
    }
}
