using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories;

public class PlayerRepository : GenericRepository<Player, int>, IPlayerRepository
{
    public PlayerRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<Player?> GetByBattlemetricsIdAsync(string battlemetricsId)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.BattlemetricsId == battlemetricsId);
    }

    public async Task<Player?> GetBySteamIdAsync(string steamId)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.SteamId == steamId);
    }
}
