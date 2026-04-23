using Microsoft.AspNetCore.Authorization;
using RaidSense.Server.Models;
using RaidSense.Server.Security.Handlers;
using RaidSense.Server.Security.Policies;
using RaidSense.Server.Security.Requirements;

namespace RaidSense.Server.Extensions;

public static class SecurityExtensions
{
    public static IServiceCollection AddAppSecurity(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.MapAdmin, policy =>
                policy.Requirements.Add(
                    new MapAccessRequirement(MapRole.Admin)));
        });

        return services;
    }
}
