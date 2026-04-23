using System;
using Microsoft.AspNetCore.Authorization;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Security.Requirements;

namespace RaidSense.Server.Security.Handlers;

public class MapAccessHandler : AuthorizationHandler<MapAccessRequirement>
{
    private readonly IMapUserService _mapUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MapAccessHandler(
        IMapUserService mapUserService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _mapUserService = mapUserService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MapAccessRequirement requirement)
    {

        var userId = context.User.GetId();

        var httpContext = _httpContextAccessor.HttpContext;

        if (userId is null || httpContext is null) 
            return;

        var mapIdValue = httpContext.Request.RouteValues["mapId"]?.ToString();

        if (!int.TryParse(mapIdValue, out var mapId))
            return;

        var hasAccess = await _mapUserService.HasRoleAsync(
            userId,
            mapId,
            requirement.RequiredRole);

        if (hasAccess)
        {
            context.Succeed(requirement);
        }

    }
}
