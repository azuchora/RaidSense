using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.UserMap;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using System.Security.Claims;

namespace RaidSense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserMapsController : ControllerBase
    {
        private readonly IUserMapService _userMapService;
        private readonly IRustMapService _rustMapService;
        public UserMapsController(IUserMapService userMapService, IRustMapService rustMapService)
        {
            _userMapService = userMapService;
            _rustMapService = rustMapService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMapDto>>> GetMaps()
        {
            var userId = User.GetId();
            var dtos = await _userMapService.GetAllDtosByOwnerAsync(userId);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserMapDto>> GetById([FromRoute] int id)
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
        public async Task<ActionResult<UserMapDto>> Create([FromBody] CreateUserMapDto dto)
        {
            var userId = User.GetId();
            var userMap = dto.ToEntity(userId);

            var newMap = await _userMapService.CreateAsync(userMap);

            return CreatedAtAction(nameof(GetById), new { id = newMap.Id }, newMap.ToDto());
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserMapDto dto)
        {
            var rustMapId = dto.MapId;
            if (rustMapId == null)
                return BadRequest("Map id is required");

            var userMap = await _userMapService.GetByIdAsync(id);
            if (userMap == null)
                return NotFound();

            var rustMap = await _rustMapService.GetOrCreateAsync(rustMapId);
            if (rustMap == null)
                return BadRequest("Map id is invalid.");

            userMap.MapId = rustMapId;
            userMap.Map = rustMap;
            
            await _userMapService.UpdateAsync(userMap);
            return Ok();
        }
    }
}
