using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Services
{
    public class RustServerService : IRustServerService
    {
        private readonly IRustServerRepository _rustServerRepo;
        private readonly IBattlemetricsService _bmService;
        private readonly IMapService _mapService;
        public RustServerService(
            IRustServerRepository rustServerRepo,
            IBattlemetricsService bmService,
            IMapService mapService)
        {
            _rustServerRepo = rustServerRepo;
            _bmService = bmService;
            _mapService = mapService;
        }

        public Task<RustServer?> GetByIdAsync(string id)
        {
            return _rustServerRepo.GetByIdAsync(id);
        }

        public async Task<RustServer?> GetOrCreateAsync(string id)
        {
            var server = await _rustServerRepo.GetByIdAsync(id);

            if (server != null)
                return server;

            var bmResponse = await _bmService.GetServerDetailsAsync(id);
            var rustServer = bmResponse?.ToRustServerEntity();
            if (rustServer == null || bmResponse == null)
                return null;

            var map = bmResponse.ToMapEntity();
            if (map == null) return null; // server doesnt support rustmaps
            
            await _mapService.CreateAsync(map);
            await _rustServerRepo.AddAndSaveAsync(rustServer);

            return rustServer;
        }

        public async Task<List<RustServer>> GetAllAsync()
        {
            var servers = await _rustServerRepo.GetAllAsync();
            return servers.ToList();
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            return await _rustServerRepo.DeleteByIdAsync(id);
        }

        public async Task<RustServer?> SyncServerAsync(string id)
        {
            var server = await _rustServerRepo.GetByIdAsync(id);

            if (server == null) return null;

            var bmServer = await _bmService.GetServerDetailsAsync(id);

            if (bmServer == null) return null;
            
            server.Name = bmServer.Name;
            server.LastFetched = DateTime.UtcNow;

            var newMap = bmServer.ToMapEntity();
            if (newMap != null && newMap.Id != server.MapId)
            {
                var existingMap = await _mapService.GetByIdAsync(newMap.Id);
                if (existingMap == null)
                    await _mapService.CreateAsync(newMap);

                server.MapId = newMap.Id;

            }
            await _rustServerRepo.UpdateAsync(server);

            return server;
        }
    }
}
