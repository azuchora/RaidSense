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
    public async Task GrantAccessAsync_ShouldAddViewer_WhenInvokerIsAdminAndTargetHasNoAccess()
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

    [Fact]
    public async Task RevokeAccessAsync_ShouldThrowForbidden_WhenInvokerIsSameAsTargetUser()
    {
        var userId = "user-1";
        var mapId = 1;

        var act = () => _service.RevokeAccessAsync(userId, userId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task RevokeAccessAsync_ShouldThrowForbidden_WhenInvokerHasNoAccess()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync((MapUser?)null);

        var act = () => _service.RevokeAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task RevokeAccessAsync__ShouldThrowForbidden_WhenInvokerIsNotAdmin()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Viewer));

        var act = () => _service.RevokeAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task RevokeAccessAsync_ShouldDoNothing_WhenTargetHasNoAccess()
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

        var act = () => _service.RevokeAccessAsync(invokerId, targetId, mapId);

        await act.Should().NotThrowAsync();

        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<MapUser>()), Times.Never);
    }

    [Fact]
    public async Task RevokeAccessAsync_ShouldThrowForbidden_WhenTargetIsOwner()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Admin));

        _repoMock
            .Setup(r => r.GetMapUserAsync(targetId, mapId))
            .ReturnsAsync(CreateMapUser(targetId, mapId, MapRole.Owner));

        var act = () => _service.RevokeAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task RevokeAccessAsync_ShouldThrowForbidden_WhenInvokerRoleEqualsTargetRole()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Admin));

        _repoMock
            .Setup(r => r.GetMapUserAsync(targetId, mapId))
            .ReturnsAsync(CreateMapUser(targetId, mapId, MapRole.Admin));

        var act = () => _service.RevokeAccessAsync(invokerId, targetId, mapId);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task RevokeAccessAsync_ShouldDelete_WhenValidRequest()
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

        var act = () => _service.RevokeAccessAsync(invokerId, targetId, mapId);

        await act.Should().NotThrowAsync();

        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<MapUser>()), Times.Once);
    }

    // -------------------------
    // UpdateRoleAsync
    // -------------------------

    [Fact]
    public async Task UpdateRoleAsync_ShouldThrowForbidden_WhenInvokerIsSameAsTargetUser()
    {
        var userId = "user-1";
        var mapId = 1;

        var act = () => _service.UpdateRoleAsync(userId, userId, mapId, MapRole.Admin);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task UpdateRoleAsync_ShouldThrowForbidden_WhenInvokerHasNoAccess()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync((MapUser?)null);

        var act = () => _service.UpdateRoleAsync(invokerId, targetId, mapId, MapRole.Admin);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task UpdateRoleAsync_ShouldThrowForbidden_WhenInvokerIsNotAdmin()
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, MapRole.Viewer));

        var act = () => _service.UpdateRoleAsync(invokerId, targetId, mapId, MapRole.Admin);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task UpdateRoleAsync_ShouldThrowForbidden_WhenTargetHasNoAccess()
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

        var act = () => _service.UpdateRoleAsync(invokerId, targetId, mapId, MapRole.Admin);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Theory]
    [InlineData(MapRole.Admin, MapRole.Viewer, MapRole.Owner)]   // new role owner
    [InlineData(MapRole.Admin, MapRole.Admin, MapRole.Viewer)]   // equal roles
    [InlineData(MapRole.Admin, MapRole.Owner, MapRole.Viewer)]    // target is owner
    public async Task UpdateRoleAsync_ShouldThrowForbidden_ForRoleRules(
        MapRole invokerRole,
        MapRole targetRole,
        MapRole newRole)
    {
        var invokerId = "invoker";
        var targetId = "user-1";
        var mapId = 1;

        _repoMock
            .Setup(r => r.GetMapUserAsync(invokerId, mapId))
            .ReturnsAsync(CreateMapUser(invokerId, mapId, invokerRole));

        _repoMock
            .Setup(r => r.GetMapUserAsync(targetId, mapId))
            .ReturnsAsync(CreateMapUser(targetId, mapId, targetRole));

        var act = () => _service.UpdateRoleAsync(invokerId, targetId, mapId, newRole);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task UpdateRoleAsync_ShouldUpdateRole_WhenValidRequest()
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

        var act = () => _service.UpdateRoleAsync(invokerId, targetId, mapId, MapRole.Editor);

        await act.Should().NotThrowAsync();

        _repoMock.Verify(r => r.UpdateAsync(It.Is<MapUser>(m =>
            m.Role == MapRole.Editor &&
            m.UserId == targetId &&
            m.MapId == mapId
        )), Times.Once);
    }
}
