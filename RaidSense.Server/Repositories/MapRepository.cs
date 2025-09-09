using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class MapRepository: GenericRepository<RustMap, string>, IMapRepository
    {
        public MapRepository(AppDbContext context) : base(context) {}
    }
}
