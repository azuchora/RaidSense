using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Controllers
{
    [Route("api/usermaps/{mapId:int}/users")]
    [ApiController]
    public class MapUsersController : ControllerBase
    {
        private readonly IMapUserService _mapUserService;
        private readonly UserManager<User> _userManager;
        public MapUsersController(IMapUserService mapUserService, UserManager<User> userManager)
        {
            _mapUserService = mapUserService;
            _userManager = userManager;
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> GrantAccess([FromRoute] string username, int mapId, [FromQuery] MapRole role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("User not found");

            if (!await _mapUserService.HasRoleAsync(user.Id, mapId, MapRole.Admin))
                return Forbid();

            var success = await _mapUserService.GrantAccessAsync(user.Id, mapId, role);

            return success ? Ok() : BadRequest();
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateRole([FromRoute] string username, int mapId, [FromQuery] MapRole role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("User not found");

            if (!await _mapUserService.HasRoleAsync(user.Id, mapId, MapRole.Admin))
                return Forbid();

            var success = await _mapUserService.UpdateRoleAsync(user.Id, mapId, role);
            return success ? Ok() : BadRequest();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> RevokeAccess([FromRoute] string username, int mapId)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("User not found");

            if (!await _mapUserService.HasRoleAsync(user.Id, mapId, MapRole.Admin))
                return Forbid();

            var success = await _mapUserService.RevokeAccessAsync(user.Id, mapId);
            return success ? Ok() : BadRequest();
        }
    }
}
