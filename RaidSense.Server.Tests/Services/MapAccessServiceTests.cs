using FluentAssertions;
using Moq;
using RaidSense.Server.Exceptions.Http;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;
using RaidSense.Server.Services;

namespace RaidSense.Server.Tests.Services;

public class MapAccessServiceTests
{
    private readonly Mock<IMapAccessRepository> _repoMock = new();
    private readonly MapAccessService _service;

    public MapAccessServiceTests()
    {
        _service = new MapAccessService(_repoMock.Object);
    }

    private static MapUser CreateMapUser(string userId, int mapId, MapRole role)
    {
        return new MapUser
        {
            UserId = userId,
            MapId = mapId,
            Role = role
        };
    }

    // -------------------------
    // HasRoleAsync
    // -------------------------

    [Fact]
    public async Task HasRoleAsync_ShouldReturnTrue_WhenUserHasHigherRole()
    {
        var userId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(userId, mapId))
            .ReturnsAsync(CreateMapUser(userId, mapId, MapRole.Admin));

        var result = await _service.HasRoleAsync(userId, mapId, MapRole.Viewer);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasRoleAsync_ShouldReturnFalse_WhenUserHasLowerRole()
    {
        var userId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(userId, mapId))
            .ReturnsAsync(CreateMapUser(userId, mapId, MapRole.Viewer));

        var result = await _service.HasRoleAsync(userId, mapId, MapRole.Admin);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task HasRoleAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        var userId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(userId, mapId))
            .ReturnsAsync((MapUser?)null);

        var result = await _service.HasRoleAsync(userId, mapId, MapRole.Admin);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task HasRoleAsync_ShouldReturnTrue_WhenUserHasEqualRole()
    {
        var userId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(userId, mapId))
            .ReturnsAsync(CreateMapUser(userId, mapId, MapRole.Admin));

        var result = await _service.HasRoleAsync(userId, mapId, MapRole.Admin);

        result.Should().BeTrue();
    }
    
    // -------------------------
    // GrantAccessAsync
    // -------------------------

    [Fact]
    public async Task GrantAccessAsync_ShouldThrowForbidden_WhenInvokerIsSameAsTargetUser()
    {
        var userId = "user-1";
        var mapId = 1;

        var act = () => _service.GrantAccessAsync(userId, userId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task GrantAccessAsync_ShouldThrowForbidden_WhenInvokerHasNoAccessToMap()
    {
        var invokerId = "invoker";
        var targetId = "user-1";

        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync((MapUser?)null);

        var act = () => _service.GrantAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task GrantAccessAsync_ShouldThrowForbidden_WhenInvokerIsNotAdmin()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Viewer));

        var act = () => _service.GrantAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task GrantAccessAsync_ShouldThrowConflict_WhenUserAlreadyHasAccess()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Admin));

        _repoMock
            .Setup(r => r.GetMapUserAsync(targetId, mapId))
            .ReturnsAsync(CreateMapUser(targetId, mapId, MapRole.Viewer));

        var act = () => _service.GrantAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ConflictException>();

    }

    [Fact]
    public async Task GrantAccessAsync_ShouldAddViewer_WhenInvokerIsAdminAndUserHasNoAccess()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Admin));

        _repoMock
            .Setup(r => r.GetMapUserAsync(targetId, mapId))
            .ReturnsAsync((MapUser?)null);

        _repoMock
            .Setup(r => r.AddAndSaveAsync(It.IsAny<MapUser>()))
            .Returns(Task.CompletedTask);

        var act = async () => await _service.GrantAccessAsync(invokerId, targetId, mapId);

        await act.Should().NotThrowAsync();

        _repoMock.Verify(r => r.AddAndSaveAsync(It.IsAny<MapUser>()), Times.Once);
    }

    // -------------------------
    // RevokeAccessAsync
    // -------------------------
}
