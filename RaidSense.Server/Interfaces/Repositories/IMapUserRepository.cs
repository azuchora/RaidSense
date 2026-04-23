using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapUserRepository : IGenericRepository<MapUser, int>
    {
        Task<MapUser?> GetMapUserAsync(string userId, int mapId);
    }
}
