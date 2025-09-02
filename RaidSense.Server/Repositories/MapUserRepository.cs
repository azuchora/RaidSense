using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class MapUserRepository : GenericRepository<MapUser, int>, IMapUserRepository
    {
        public MapUserRepository(AppDbContext context) : base(context) { }
    }
}
