using System;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services;

public class PlayerService : IPlayerService
{
    private readonly IBattlemetricsService _bmService;
    private readonly IPlayerRepository _playerRepo;
    public PlayerService(IBattlemetricsService bmService, IPlayerRepository playerRepo)
    {
        _bmService = bmService;
        _playerRepo = playerRepo;
    }

    public async Task<Player?> GetOrCreateByBattlemetricsIdAsync(string battlemetricsId)
    {
        var foundPlayer = await _playerRepo.GetByBattlemetricsIdAsync(battlemetricsId);
        if (foundPlayer != null)
            return foundPlayer;

        var newPlayer = await CreateByBattlemetricsIdAsync(battlemetricsId);
        return newPlayer;
    }

    public async Task<Player?> GetByBattlemetricsIdAsync(string battlemetricsId)
    {
        return await _playerRepo.GetByBattlemetricsIdAsync(battlemetricsId);
    }

    public Task<Player?> UpdateByBattlemetricsIdAsync(string battlemetricsId)
    {
        throw new NotImplementedException();
    }

    public Task<Player?> CreateByBattlemetricsIdAsync(string battlemetricsId)
    {
        throw new NotImplementedException();
    }
}
