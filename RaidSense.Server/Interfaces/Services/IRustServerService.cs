using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IRustServerService
    {
        Task<RustServer?> GetOrCreateAsync(string id);
        Task<RustServer?> GetByIdAsync(string id);
        Task<List<RustServer>> GetAllAsync();
    }
}
