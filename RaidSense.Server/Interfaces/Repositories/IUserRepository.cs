using System;
using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByRefreshTokenAsync(string token);
}
