using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IRustMapService
    {
        Task<RustMap> GetByIdAsync(string id);
        Task<List<RustMap>> GetAllAsync();
        Task DeleteByIdAsync(string id);
        Task<RustMap> CreateAsync(RustMap map);
        Task<RustMap> EnsureExistsAsync(string id);
        Task<RustMap> SyncMapAsync(string id);
    }
}
