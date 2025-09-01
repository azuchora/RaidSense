using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepo;
        private readonly IRustMapsService _rustMapsService;
        public MapService(IMapRepository mapRepo, IRustMapsService rustMapsService)
        {
            _mapRepo = mapRepo;
            _rustMapsService = rustMapsService;
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

        public async Task<Map?> GetOrCreateAsync(string id)
        {
            var map = await _mapRepo.GetByIdAsync(id);
            if (map != null) return map;

            var newMap = (await _rustMapsService.GetRustMapDetailsAsync(id))?.ToEntity();
            if(newMap == null) return null;

            await _mapRepo.AddAndSaveAsync(newMap);
            return newMap;
        }

        public async Task<Map?> SyncMapAsync(string id)
        {
            var map = await _mapRepo.GetByIdAsync(id);

            var rustMapsDto = await _rustMapsService.GetRustMapDetailsAsync(id);
            if (rustMapsDto == null) return null;

            if (map == null)
            {
                var updatedMap = rustMapsDto.ToEntity();
                await _mapRepo.AddAndSaveAsync(updatedMap);
                return updatedMap;
            }

            map.UpdateFromDto(rustMapsDto);

            await _mapRepo.UpdateAsync(map);
            return map;
        }
    }
}
