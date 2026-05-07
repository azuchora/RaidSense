using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Exceptions.Http;

namespace RaidSense.Server.Services
{
    public class UserMapService : IUserMapService
    {
        private readonly IUserMapRepository _userMapRepo;
        private readonly IMapAccessService _mapAccessService;
        private readonly IRustMapService _rustMapService;
        public UserMapService(IUserMapRepository userMapRepo, IMapAccessService mapAccessService, IRustMapService rustMapService)
        {
            _userMapRepo = userMapRepo;
            _mapAccessService = mapAccessService;
            _rustMapService = rustMapService;
        }

        public async Task<List<UserMap>> GetAllAccesibleAsync(string invokerId, MapRole minimumRole)
        {
            var ids = await _mapAccessService.GetAccessibleMapIdsAsync(invokerId, minimumRole);
            var maps = await _userMapRepo.GetByIdsAsync(ids);
            return maps;
        }

        public async Task<UserMap> CreateAsync(UserMap userMap)
        {
            await _userMapRepo.AddAndSaveAsync(userMap);
            await _mapAccessService.AssignOwnerAsync(userMap.OwnerId, userMap.Id);
            return await GetByIdDetailedAsync(userMap.OwnerId, userMap.Id);
        }

        public async Task DeleteByIdAsync(string invokerId, int mapId)
        {
            await _mapAccessService.EnsureOwnerAsync(invokerId, mapId);
            await _userMapRepo.DeleteByIdAsync(mapId);
        }


        public async Task<UserMap> GetByIdDetailedAsync(string invokerId, int mapId)
        {
            await _mapAccessService.EnsureViewerAsync(invokerId, mapId);

            var map = await _userMapRepo.GetByIdDetailedAsync(mapId);
            return map ?? throw new NotFoundException("Map not found");
        }

        public async Task UpdateAsync(string invokerId, int mapId, string rustMapId)
        {
            await _mapAccessService.EnsureAdminAsync(invokerId, mapId);

            var userMap = await GetByIdAsync(mapId);

            var rustMap = await _rustMapService.EnsureExistsAsync(rustMapId);

            userMap.MapId = rustMapId;
            userMap.Map = rustMap;
            
            await _userMapRepo.UpdateAsync(userMap);
        }

        private async Task<UserMap> GetByIdAsync(int id)
        {
            var map = await _userMapRepo.GetByIdAsync(id);
            return map ?? throw new NotFoundException("Map not found");
        }
    }
}
