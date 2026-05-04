using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IUserMapService
    {
        Task<UserMap> GetByIdDetailedAsync(string invokerId, int mapId);
        Task<UserMap> CreateAsync(UserMap userMap);
        Task<List<UserMap>> GetAllAccesibleAsync(string invokerId, MapRole minimumRole);
        Task DeleteByIdAsync(string invokerId, int mapId);
        Task UpdateAsync(string invokerId, int mapId, string rustMapId);
    }
}
