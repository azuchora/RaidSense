using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}
