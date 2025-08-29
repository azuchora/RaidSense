using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> RotateTokenAsync(User user, string? existingTokenValue, string ipAddress);
        Task RevokeTokenAsync(User user, string tokenValue, string ipAddress);
    }
}
