using Microsoft.AspNetCore.Identity;
using RaidSense.Server.Interfaces;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        public RefreshTokenService(ITokenService tokenService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<RefreshToken> RotateTokenAsync(User user, string? existingTokenValue, string ipAddress)
        {
            if(!string.IsNullOrEmpty(existingTokenValue))
            {
                var oldToken = user.RefreshTokens
                    .SingleOrDefault(t => t.Token == existingTokenValue && t.IsActive);

                if(oldToken != null)
                {
                    oldToken.Revoked = DateTime.UtcNow;
                    oldToken.RevokedByIp = ipAddress;
                }
            }
            
            var newRefreshToken = _tokenService.GenerateRefreshToken(user, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            return newRefreshToken;
        }

        public async Task RevokeTokenAsync(User user, string tokenValue, string ipAddress)
        {
            var token = user.RefreshTokens.SingleOrDefault(t => t.Token == tokenValue && t.IsActive);
            if (token != null)
            {
                token.Revoked = DateTime.UtcNow;
                token.RevokedByIp = ipAddress;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
