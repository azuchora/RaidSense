using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapService
    {
        Task<Map?> GetByIdAsync(string id);
        Task<List<Map>> GetAllAsync();
        Task<bool> DeleteByIdAsync(string id);
        Task<Map> CreateAsync(Map map);
        Task<Map?> GetOrCreateAsync(string id);
        Task<Map?> SyncMapAsync(string id);
    }
}
