using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Dtos.UserMap;
using RaidSense.Server.Interfaces.Services;
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

        public async Task<UserMap> CreateAsync(UserMap userMap)
        {
            await _userMapRepo.AddAndSaveAsync(userMap);
            await _mapUserService.GrantAccessAsync(userMap.OwnerId, userMap.Id, MapRole.Owner);
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

        public async Task<bool> DeleteIfOwnerAsync(int mapId, string userId)
        {
            var map = await GetByIdAsync(mapId);
            if (map == null || map.OwnerId != userId)
                return false;

            await DeleteByIdAsync(mapId);
            return true;
        }

        public async Task<bool> UpdateRustMapAsync(int id, string newRustMapId)
        {
            var map = await GetByIdAsync(id);
            if (map == null)
                return false;

            map.MapId = newRustMapId;
            await _userMapRepo.UpdateAsync(map);
            return true;
        }

        public async Task UpdateAsync(UserMap userMap)
        {
            await _userMapRepo.UpdateAsync(userMap);
        }
    }
}
