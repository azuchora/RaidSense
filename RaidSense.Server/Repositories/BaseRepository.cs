using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class BaseRepository : GenericRepository<Base, int>, IBaseRepository
    {
        public BaseRepository(AppDbContext context) : base(context) { }
    }
}
