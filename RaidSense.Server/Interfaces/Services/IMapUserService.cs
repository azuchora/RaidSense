using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapUserService
    {
        Task GrantAccessAsync(string invokerId, string userId, int userMapId);
        Task RevokeAccessAsync(string invokerId, string userId, int userMapId);
        Task UpdateRoleAsync(string invokerId, string userId, int userMapId, MapRole newRole);
        Task<bool> HasRoleAsync(string userId, int mapId, MapRole minimumRole);
         Task AssignOwnerAsync(string userId, int mapId);
    }
}
