using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
