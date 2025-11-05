using System;
using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Repositories;

public interface IPlayerRepository : IGenericRepository<Player, int>
{
    Task<Player?> GetByBattlemetricsIdAsync(string battlemetricsId);
    Task<Player?> GetBySteamIdAsync(string steamId);
}
