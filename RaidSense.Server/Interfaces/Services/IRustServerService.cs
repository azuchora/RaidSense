using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IRustServerService
    {
        Task<RustServer> EnsureExistsAsync(string id);
        Task<RustServer> GetByIdAsync(string id);
        Task<List<RustServer>> GetAllAsync();
        Task DeleteByIdAsync(string id);
        Task<RustServer> SyncServerAsync(string id);
    }
}
