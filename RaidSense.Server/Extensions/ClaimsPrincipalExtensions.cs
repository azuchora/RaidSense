using System.Security.Claims;

namespace RaidSense.Server.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated");
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.Identity?.Name ?? throw new UnauthorizedAccessException("User is not authrnticated");
        }
    }
}
