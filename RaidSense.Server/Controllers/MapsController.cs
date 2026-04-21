using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.Map;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly IRustMapService _mapService;
        public MapsController(IRustMapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapDto>>> GetAll()
        {
            var maps = await _mapService.GetAllAsync();
            var dtos = maps.Select(map => map.ToDto());

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MapDto>> GetById([FromRoute] string id)
        {
            var map = await _mapService.GetByIdAsync(id);

            return Ok(map.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MapDto>> EnsureExists([FromRoute] string id)
        {
            var map = await _mapService.EnsureExistsAsync(id);

            var dto = map.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = map.Id }, dto);
        }

        [HttpPost("{id}/sync")]
        public async Task<ActionResult<MapDto>> SyncMap([FromRoute] string id)
        {
            var map = await _mapService.SyncMapAsync(id);

            return Ok(map.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            await _mapService.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
