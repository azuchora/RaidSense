using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Controllers
{
    [Route("api/usermaps/{mapId:int}/users")]
    [ApiController]
    [Authorize]
    public class MapUsersController : ControllerBase
    {
        private readonly IMapUserService _mapUserService;
        private readonly UserManager<User> _userManager;
        public MapUsersController(IMapUserService mapUserService, UserManager<User> userManager)
        {
            _mapUserService = mapUserService;
            _userManager = userManager;
        }

        private async Task<(string? userId, IActionResult? errorResult)> ValidateUserAndRolesAsync(string username, int mapId, MapRole? newRole)
        {
            var invokerId = User.GetId();

            var foundUser = await _userManager.FindByNameAsync(username);
            if (foundUser == null)
                return (null, NotFound("User not found"));

            if (!await _mapUserService.HasRoleAsync(invokerId, mapId, MapRole.Admin))
                return (null, Forbid());

            if (newRole != null && newRole > MapRole.Admin)
                return (null, BadRequest("You can only assign roles up to Admin"));

            return (foundUser.Id, null);
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> GrantAccess([FromRoute] string username, int mapId, [FromQuery] MapRole role)
        {
            var (userId, error) = await ValidateUserAndRolesAsync(username, mapId, role);
            if (error != null) return error;

            var success = await _mapUserService.GrantAccessAsync(userId!, mapId, role);

            return success ? Ok() : BadRequest();
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateRole([FromRoute] string username, int mapId, [FromQuery] MapRole role)
        {
            var (userId, error) = await ValidateUserAndRolesAsync(username, mapId, role);
            if (error != null) return error;

            var success = await _mapUserService.UpdateRoleAsync(userId!, mapId, role);
            return success ? Ok() : BadRequest();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> RevokeAccess([FromRoute] string username, int mapId)
        {
            var (userId, error) = await ValidateUserAndRolesAsync(username, mapId, null);
            if (error != null) return error;

            var success = await _mapUserService.RevokeAccessAsync(userId!, mapId);
            return success ? Ok() : BadRequest();
        }
    }
}
