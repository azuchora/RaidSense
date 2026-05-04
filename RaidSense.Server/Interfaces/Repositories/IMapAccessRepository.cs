using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapAccessRepository : IGenericRepository<MapUser, int>
    {
        Task<MapUser?> GetMapUserAsync(string userId, int mapId);
        Task<List<int>> GetAccessibleMapIdsAsync(string userId, MapRole minimumRole);
    }
}
