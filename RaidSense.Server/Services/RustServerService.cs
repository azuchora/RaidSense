using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;
using RaidSense.Server.Mappers;
using RaidSense.Server.Exceptions.Http;
using RaidSense.Server.Dtos.BattleMetrics.Server;

namespace RaidSense.Server.Services
{
    public class RustServerService : IRustServerService
    {
        private readonly IRustServerRepository _rustServerRepo;
        private readonly IBattlemetricsService _bmService;
        private readonly IRustMapService _mapService;
        public RustServerService(
            IRustServerRepository rustServerRepo,
            IBattlemetricsService bmService,
            IRustMapService mapService)
        {
            _rustServerRepo = rustServerRepo;
            _bmService = bmService;
            _mapService = mapService;
        }

        public async Task<RustServer> GetByIdAsync(string id)
        {
            var server = await _rustServerRepo.GetByIdAsync(id);
            return server ?? throw new NotFoundException("Server not found");
        }

        public async Task<RustServer> EnsureExistsAsync(string id)
        {
            var existing = await _rustServerRepo.GetByIdAsync(id);

            if (existing is not null)
                return existing;

            var bmServer = await GetBmServerOrThrowAsync(id);

            var map = bmServer?.ToMapEntity() 
                ?? throw new NotFoundException("Server does not support rustmaps");
            
            var server = bmServer.ToRustServerEntity();

            await _mapService.CreateAsync(map);
            await _rustServerRepo.AddAndSaveAsync(server);

            return server;
        }

        public async Task<List<RustServer>> GetAllAsync()
        {
            return await _rustServerRepo.GetAllAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var deleted = await _rustServerRepo.DeleteByIdAsync(id);

            if(!deleted) throw new NotFoundException("Server not found");
        }

        public async Task<RustServer> SyncServerAsync(string id)
        {
            var server = await GetByIdAsync(id);
            var bmServer = await GetBmServerOrThrowAsync(id);

            server.Name = bmServer.Name;
            server.LastFetched = DateTime.UtcNow;

            await SyncMap(server, bmServer);
            await _rustServerRepo.UpdateAsync(server);

            return server;
        }

        private async Task SyncMap(RustServer server, BmServerDto bmServer)
        {
            var newMap = bmServer.ToMapEntity();

            if(newMap is null || newMap.Id == server.MapId)
                return;

            try
            {
                await _mapService.GetByIdAsync(newMap.Id);
            }
            catch (NotFoundException)
            {
                await _mapService.CreateAsync(newMap);
            }

            server.MapId = newMap.Id;
        }

        private async Task<BmServerDto> GetBmServerOrThrowAsync(string id)
        {
            var server = await _bmService.GetServerDetailsAsync(id);

            return server ?? throw new NotFoundException("Server not found in BattleMetrics");
        }
    }
}
