using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IMapService
    {
        Task<RustMap?> GetByIdAsync(string id);
        Task<List<RustMap>> GetAllAsync();
        Task<bool> DeleteByIdAsync(string id);
        Task<RustMap> CreateAsync(RustMap map);
        Task<RustMap?> GetOrCreateAsync(string id);
        Task<RustMap?> SyncMapAsync(string id);
    }
}
