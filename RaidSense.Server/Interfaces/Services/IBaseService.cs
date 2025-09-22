using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IBaseService
    {
        Task<Base?> GetByIdAsync(int id);
        Task<Base?> GetByIdDetailedAsync(int id);
        Task<Base> CreateAsync(Base newBase);
        Task UpdateAsync(Base newBase);
        Task<bool> DeleteAsync(int id);
    }
}
