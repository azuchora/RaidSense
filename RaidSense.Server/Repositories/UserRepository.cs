using System;
using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Data;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Models;

namespace RaidSense.Server.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByRefreshTokenAsync(string token)
    {
        return await _context.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u =>
                u.RefreshTokens.Any(t => t.Token == token));
    }
}
