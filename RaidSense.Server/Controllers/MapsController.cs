using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.Maps;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models;
using RaidSense.Server.Security.Policies;

namespace RaidSense.Server.Controllers
{
    [Route("api/maps")]
    [ApiController]
    [Authorize]
    public class MapsController : ControllerBase
    {
        private readonly IUserMapService _userMapService;
        public MapsController(IUserMapService userMapService)
        {
            _userMapService = userMapService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapDto>>> GetMaps()
        {
            var maps = await _userMapService.GetAllAccesibleAsync(GetInvokerId(), MapRole.Viewer);
            var dtos = maps.Select(m => m.ToDto()).ToList();
            return Ok(dtos);
        }

        [HttpGet("{mapId:int}")]
        [Authorize(Policy = PolicyNames.MapViewer)]
        public async Task<ActionResult<MapDto>> GetById([FromRoute] int mapId)
        {
            var map = await _userMapService.GetByIdDetailedAsync(User.GetId(), mapId);

            return Ok(map.ToDto());
        }

        [HttpDelete("{mapId:int}")]
        [Authorize(Policy = PolicyNames.MapOwner)]
        public async Task<IActionResult> Delete([FromRoute] int mapId)
        {
            await _userMapService.DeleteByIdAsync(User.GetId(), mapId);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MapDto>> Create([FromBody] CreateMapDto dto)
        {
            var map = dto.ToEntity(GetInvokerId());

            var newMap = await _userMapService.CreateAsync(map);

            return CreatedAtAction(nameof(GetById), new { mapId = newMap.Id }, newMap.ToDto());
        }

        [HttpPatch("{mapId:int}")]
        [Authorize(Policy = PolicyNames.MapAdmin)]
        public async Task<IActionResult> Update([FromRoute] int mapId, [FromBody] UpdateMapDto dto)
        {
            await _userMapService.UpdateAsync(User.GetId(), mapId, dto.MapId);
            return NoContent();
        }

        private string GetInvokerId()
        {
            return User.GetId();
        }
    }
}
