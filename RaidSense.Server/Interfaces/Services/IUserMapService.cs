using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IUserMapService
    {
        Task<UserMap?> GetByIdAsync(int id);
        Task<UserMap?> GetByIdDetailedAsync(int id);
        Task<List<UserMap>> GetAllByOwnerAsync(string ownerId);
        Task<UserMap> CreateAsync(UserMap userMap);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> AddUserAccessAsync(string userId, int mapId, MapRole role);
        Task<bool> RemoveUserAccessAsync(string userId, int mapId);
        Task<bool> UpdateUserRoleAsync(string userId, int mapId, MapRole role);
        Task AddBaseAsync(int mapId, Base newBase);
    }
}
