using System;
using Microsoft.AspNetCore.Authorization;
using RaidSense.Server.Models;

namespace RaidSense.Server.Security.Requirements;

public class MapAccessRequirement : IAuthorizationRequirement
{
    public MapRole RequiredRole { get; }

    public MapAccessRequirement(MapRole requiredRole)
    {
        RequiredRole = requiredRole;
    }
}
