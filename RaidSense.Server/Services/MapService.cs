using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepo;
        public MapService(IMapRepository mapRepo)
        {
            _mapRepo = mapRepo;
        }

        public async Task<Map> CreateAsync(Map map)
        {
            await _mapRepo.AddAndSaveAsync(map);
            return map;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            return await _mapRepo.DeleteByIdAsync(id);
        }

        public async Task<List<Map>> GetAllAsync()
        {
            var maps = await _mapRepo.GetAllAsync();
            return maps.ToList();
        }

        public async Task<Map?> GetByIdAsync(string id)
        {
            return await _mapRepo.GetByIdAsync(id);
        }
    }
}
