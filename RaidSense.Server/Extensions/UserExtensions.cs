using RaidSense.Server.Models;

namespace RaidSense.Server.Extensions
{
    public static class UserExtensions
    {
        public static RefreshToken? GetActiveToken(this User user, string tokenValue)
            => user.RefreshTokens.SingleOrDefault(t => t.Token == tokenValue && t.IsActive);
    }
}
