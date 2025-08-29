namespace RaidSense.Server.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetIpAddress(this HttpContext context)
            => context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
