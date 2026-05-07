using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class MapAccessRepository : GenericRepository<MapUser, int>, IMapAccessRepository
    {
        public MapAccessRepository(AppDbContext context) : base(context) { }

        public async Task<List<int>> GetAccessibleMapIdsAsync(string userId, MapRole minimumRole)
        {
            return await GetQueryable()
                .Where(x => x.UserId == userId && x.Role >= minimumRole)
                .Select(x => x.MapId)
                .ToListAsync();
        }

        public async Task<MapUser?> GetMapUserAsync(string userId, int mapId)
        {
            return await GetQueryable()
                .FirstOrDefaultAsync(mu => mu.UserId == userId && mu.MapId == mapId);
        }
    }
}
