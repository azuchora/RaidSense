using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories
{
    public class UserMapRepository : GenericRepository<UserMap, int>, IUserMapRepository
    {
        public UserMapRepository(AppDbContext context) : base(context) { }
    }
}
