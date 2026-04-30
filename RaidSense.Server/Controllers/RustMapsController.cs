using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.RustMaps;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Controllers
{
    [Authorize]
    [Route("api/rustmaps")]
    [ApiController]
    public class RustMapsController : ControllerBase
    {
        private readonly IRustMapService _rustMapService;
        public RustMapsController(IRustMapService rustMapService)
        {
            _rustMapService = rustMapService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RustMapDto>>> GetAll()
        {
            var maps = await _rustMapService.GetAllAsync();
            var dtos = maps.Select(map => map.ToDto());

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RustMapDto>> GetById([FromRoute] string id)
        {
            var map = await _rustMapService.GetByIdAsync(id);

            return Ok(map.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RustMapDto>> EnsureExists([FromRoute] string id)
        {
            var map = await _rustMapService.EnsureExistsAsync(id);

            var dto = map.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = map.Id }, dto);
        }

        [HttpPost("{id}/sync")]
        public async Task<ActionResult<RustMapDto>> SyncMap([FromRoute] string id)
        {
            var map = await _rustMapService.SyncMapAsync(id);

            return Ok(map.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            await _rustMapService.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
