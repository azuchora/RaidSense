using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken(User user, string ipAddress);
        void SetRefreshTokenCookie(HttpResponse response, RefreshToken refreshToken);
        void DeleteRefreshTokenCookie(HttpResponse response);
    }
}
