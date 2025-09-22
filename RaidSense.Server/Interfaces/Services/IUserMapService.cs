using RaidSense.Server.Dtos.UserMap;
using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IUserMapService
    {
        Task<UserMap?> GetByIdAsync(int id);
        Task<UserMap?> GetByIdDetailedAsync(int id);
        Task<List<UserMap>> GetAllByOwnerAsync(string ownerId);
        Task<List<UserMapDto>> GetAllDtosByOwnerAsync(string ownerId);
        Task<UserMap> CreateAsync(UserMap userMap);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> DeleteIfOwnerAsync(int mapId, string userId);
        Task<bool> UpdateRustMapAsync(int id, string newRustMapId);
        Task UpdateAsync(UserMap userMap);
    }
}
