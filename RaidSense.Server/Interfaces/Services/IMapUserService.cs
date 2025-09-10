using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapUserService
    {
        Task<bool> GrantAccessAsync(string userId, int userMapId, MapRole role);
        Task<bool> RevokeAccessAsync(string userId, int userMapId);
        Task<bool> UpdateRoleAsync(string userId, int userMapId, MapRole newRole);
        Task<bool> HasRoleAsync(string userId, int mapId, MapRole minimumRole);
    }
}
