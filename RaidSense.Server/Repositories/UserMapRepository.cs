using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class UserMapRepository : GenericRepository<UserMap, int>, IUserMapRepository
    {
        public UserMapRepository(AppDbContext context) : base(context) { }

        public async Task<UserMap?> GetByIdDetailedAsync(int id)
        {
            var query = GetQueryable();
            return await query
                .Include(um => um.Map)
                .Include(um => um.MapUsers)
                    .ThenInclude(mu => mu.User)
                .SingleOrDefaultAsync(um => um.Id == id);
        }

        public async Task<List<UserMap>> GetByIdsAsync(List<int> ids)
        {
            return await GetQueryable()
                .Where(um => ids.Contains(um.Id))
                .Include(um => um.Map)
                .Include(um => um.MapUsers)
                    .ThenInclude(mu => mu.User)
                .ToListAsync();
        }
    }
}
