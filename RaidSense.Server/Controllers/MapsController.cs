using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.Maps;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Controllers
{
    [Route("api/maps")]
    [ApiController]
    [Authorize]
    public class MapsController : ControllerBase
    {
        private readonly IUserMapService _userMapService;
        private readonly IRustMapService _rustMapService;
        public MapsController(IUserMapService userMapService, IRustMapService rustMapService)
        {
            _userMapService = userMapService;
            _rustMapService = rustMapService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapDto>>> GetMaps()
        {
            var userId = User.GetId();
            var dtos = await _userMapService.GetAllDtosByOwnerAsync(userId);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MapDto>> GetById([FromRoute] int id)
        {
            var map = await _userMapService.GetByIdDetailedAsync(id);
            if (map == null)
                return NotFound();

            return Ok(map.ToDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = User.GetId();
            var deleted = await _userMapService.DeleteIfOwnerAsync(id, userId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MapDto>> Create([FromBody] CreateMapDto dto)
        {
            var userId = User.GetId();
            var userMap = dto.ToEntity(userId);

            var newMap = await _userMapService.CreateAsync(userMap);

            return CreatedAtAction(nameof(GetById), new { id = newMap.Id }, newMap.ToDto());
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMapDto dto)
        {
            var rustMapId = dto.MapId;
            if (rustMapId == null)
                return BadRequest("Map id is required");

            var userMap = await _userMapService.GetByIdAsync(id);
            if (userMap == null)
                return NotFound();

            var rustMap = await _rustMapService.EnsureExistsAsync(rustMapId);
            if (rustMap == null)
                return BadRequest("Map id is invalid.");

            userMap.MapId = rustMapId;
            userMap.Map = rustMap;
            
            await _userMapService.UpdateAsync(userMap);
            return Ok();
        }
    }
}
