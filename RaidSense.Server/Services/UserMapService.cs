using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Dtos.UserMap;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models;
using RaidSense.Server.Interfaces.Repositories;

namespace RaidSense.Server.Services
{
    public class UserMapService : IUserMapService
    {
        private readonly IUserMapRepository _userMapRepo;
        private readonly IMapUserService _mapUserService;
        public UserMapService(IUserMapRepository userMapRepo, IMapUserService mapUserService)
        {
            _userMapRepo = userMapRepo;
            _mapUserService = mapUserService;
        }

        public Task AddBaseAsync(int mapId, Base newBase)
        {
            throw new NotImplementedException();
        }

        public async Task<UserMap> CreateAsync(UserMap userMap, string ownerId)
        {
            await _userMapRepo.AddAndSaveAsync(userMap);
            await _mapUserService.GrantAccessAsync(ownerId, userMap.Id, MapRole.Owner);
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

        public async Task<bool> AddUserAccessAsync(string userId, int mapId, MapRole role)
        {
            return await _mapUserService.GrantAccessAsync(userId, mapId, role);
        }

        public async Task<bool> RemoveUserAccessAsync(string userId, int mapId)
        {
            return await _mapUserService.RevokeAccessAsync(userId, mapId);
        }
         
        public async Task<bool> UpdateUserAccessAsync(string userId, int mapId, MapRole role)
        {
            return await _mapUserService.UpdateRoleAsync(userId, mapId, role);
        }

        public async Task<List<UserMapDto>> GetAllDtosByOwnerAsync(string ownerId)
        {
            return await _userMapRepo.GetQueryable()
                .Where(um => um.OwnerId == ownerId)
                .Include(um => um.Map)
                .Include(um => um.MapUsers)
                .Include(um => um.Bases)
                .Select(um => new UserMapDto
                {
                    Id = um.Id,
                    OwnerId = um.OwnerId,
                    Map = um.Map,
                    MapUsers = um.MapUsers.ToList(),
                    Bases = um.Bases.ToList(),
                })
                .ToListAsync();
        }
    }
}
