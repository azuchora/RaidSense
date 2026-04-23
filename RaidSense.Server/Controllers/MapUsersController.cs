using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;
using RaidSense.Server.Security.Policies;

namespace RaidSense.Server.Controllers
{
    [Authorize]
    [Route("api/usermaps/{mapId:int}/users")]
    [ApiController]
    public class MapUsersController : ControllerBase
    {
        private readonly IMapUserService _mapUserService;
        public MapUsersController(IMapUserService mapUserService)
        {
            _mapUserService = mapUserService;
        }

        [HttpPost("{userId}")]
        [Authorize(Policy = PolicyNames.MapAdmin)]
        public async Task<IActionResult> GrantAccess([FromRoute] string userId, int mapId)
        {
            await _mapUserService.GrantAccessAsync(GetInvokerId(), userId, mapId);
            return Ok();
        }

        [HttpPut("{userId}")]
        [Authorize(Policy = PolicyNames.MapAdmin)]
        public async Task<IActionResult> UpdateRole([FromRoute] string userId, int mapId, [FromQuery] MapRole role)
        {
            await _mapUserService.UpdateRoleAsync(GetInvokerId(), userId, mapId, role);
            return Ok();
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = PolicyNames.MapAdmin)]
        public async Task<IActionResult> RevokeAccess([FromRoute] string userId, int mapId)
        {
            await _mapUserService.RevokeAccessAsync(GetInvokerId(), userId, mapId);
            return Ok();
        }

        private string GetInvokerId()
        {
            return User.GetId();
        }
    }
}
