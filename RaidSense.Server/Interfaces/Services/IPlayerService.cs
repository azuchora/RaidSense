using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services;

public interface IPlayerService
{
    Task<Player?> GetByBattlemetricsIdAsync(string battlemetricsId);
    Task<Player?> GetOrCreateByBattlemetricsIdAsync(string battlemetricsId);
    Task<Player?> UpdateByBattlemetricsIdAsync(string battlemetricsId);
    Task<Player?> CreateByBattlemetricsIdAsync(string battlemetricsId);
}
