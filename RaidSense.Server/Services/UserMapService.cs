using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;
using RaidSense.Server.Repositories;

namespace RaidSense.Server.Services
{
    public class UserMapService : IUserMapService
    {
        private readonly UserMapRepository _userMapRepo;
        private readonly IMapUserService _mapUserService;
        public UserMapService(UserMapRepository userMapRepo, IMapUserService mapUserService)
        {
            _userMapRepo = userMapRepo;
            _mapUserService = mapUserService;
        }

        public Task AddBaseAsync(int mapId, Base newBase)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserAccessAsync(string userId, int mapId, MapRole role)
        {
            return await _mapUserService.GrantAccessAsync(userId, mapId, role);
        }

        public async Task<UserMap> CreateAsync(UserMap userMap)
        {
            await _userMapRepo.AddAndSaveAsync(userMap);
            return userMap;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await _userMapRepo.DeleteByIdAsync(id);
        }

        public async Task<List<UserMap>> GetAllByOwnerAsync(string ownerId)
        {
            var query = _userMapRepo.GetQueryable();

            var userMaps = await query
                .Where(um => um.OwnerId == ownerId)
                .ToListAsync();

            return userMaps;
        }

        public async Task<UserMap?> GetByIdAsync(int id)
        {
            return await _userMapRepo.GetByIdAsync(id);
        }

        public async Task<UserMap?> GetByIdDetailedAsync(int id)
        {
            var query = _userMapRepo.GetQueryable();

            var userMap = await query
                .Include(um => um.Map)
                .Include(um => um.Bases)
                .Include(um => um.MapUsers)
                .SingleOrDefaultAsync(um => um.Id == id);

            return userMap;
        }

        public async Task<bool> RemoveUserAccessAsync(string userId, int mapId)
        {
            return await _mapUserService.RevokeAccessAsync(userId, mapId);
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, int mapId, MapRole role)
        {
            return await _mapUserService.UpdateRoleAsync(userId, mapId, role);
        }
    }
}
