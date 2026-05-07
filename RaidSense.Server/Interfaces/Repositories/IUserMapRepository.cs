using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Repositories
{
    public interface IUserMapRepository : IGenericRepository<UserMap, int>
    {
        public Task<UserMap?> GetByIdDetailedAsync(int id);
        public Task<List<UserMap>> GetByIdsAsync(List<int> ids);
    }
}
