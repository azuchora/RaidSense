using RaidSense.Server.Dtos.BattleMetrics.Server;

namespace RaidSense.Server.Interfaces.Services
{
    public interface IBattlemetricsService
    {
        Task<BmServerDto?> GetServerDetailsAsync(string serverId);
    }
}
