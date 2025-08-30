using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class RustServerRepository: GenericRepository<RustServer, string>, IRustServerRepository
    {
        public RustServerRepository(AppDbContext context) : base(context){ }
    }
}
