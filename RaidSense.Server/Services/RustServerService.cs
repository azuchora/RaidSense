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
        public RustServerService(IRustServerRepository rustServerRepo, IBattlemetricsService bmService)
        {
            _rustServerRepo = rustServerRepo;
            _bmService = bmService;
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

            var rustServer = (await _bmService.GetServerDetailsAsync(id))?.ToRustServer();
            if (rustServer == null)
                return null;

            // ToDo - handle potential map/s before adding server
            await _rustServerRepo.AddAndSaveAsync(rustServer);

            return rustServer;
        }

        public async Task<List<RustServer>> GetAllAsync()
        {
            var servers = await _rustServerRepo.GetAllAsync();
            return servers.ToList();
        }
    }
}
