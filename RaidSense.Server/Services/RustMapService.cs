using RaidSense.Server.Dtos.RustMaps;
using RaidSense.Server.Exceptions.Http;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class RustMapService : IRustMapService
    {
        private readonly IMapRepository _mapRepo;
        private readonly IRustMapsApiService _rustMapsService;
        public RustMapService(IMapRepository mapRepo, IRustMapsApiService rustMapsService)
        {
            _mapRepo = mapRepo;
            _rustMapsService = rustMapsService;
        }

        public async Task<RustMap> CreateAsync(RustMap map)
        {
            await _mapRepo.AddAndSaveAsync(map);
            return map;
        }

        public async Task DeleteByIdAsync(string id)
        {
            var result = await _mapRepo.DeleteByIdAsync(id);

            if(!result) 
                throw new NotFoundException("Map not found");
        }

        public async Task<List<RustMap>> GetAllAsync()
        {
            return await _mapRepo.GetAllAsync();
        }

        public async Task<RustMap> GetByIdAsync(string id)
        {
            var map = await _mapRepo.GetByIdAsync(id); 

            return map ?? throw new NotFoundException("Map not found");
        }

        public async Task<RustMap> EnsureExistsAsync(string id)
        {
            var existing = await _mapRepo.GetByIdAsync(id);
            if (existing is not null) return existing;

            var external = await GetExternalMapOrThrowAsync(id);
            var newMap = external.ToEntity();
            
            await _mapRepo.AddAndSaveAsync(newMap);
            return newMap;
        }

        public async Task<RustMap> SyncMapAsync(string id)
        {
            var map = await _mapRepo.GetByIdAsync(id);

            var external = await GetExternalMapOrThrowAsync(id);
            
            if (map is null)
            {
                var updatedMap = external.ToEntity();
                await _mapRepo.AddAndSaveAsync(updatedMap);
                return updatedMap;
            }

            map.UpdateFromDto(external);

            await _mapRepo.UpdateAsync(map);
            return map;
        }

        private async Task<RustMapsDataDto> GetExternalMapOrThrowAsync(string id)
        {
            var dto = await _rustMapsService.GetRustMapDetailsAsync(id);

            return dto ?? throw new NotFoundException("Map not found in rustmaps");
        }
    }
}
