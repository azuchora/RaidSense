using Microsoft.AspNetCore.Http;
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
        private readonly IMapService _mapService;
        public MapsController(IMapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MapDto>>> GetAll()
        {
            var maps = await _mapService.GetAllAsync();
            var dtos = maps.Select(map => map.ToDto()).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MapDto>> GetById([FromRoute] string id)
        {
            var map = await _mapService.GetByIdAsync(id);

            if (map == null)
                return NotFound();

            return Ok(map.ToDto());
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<MapDto>> CreateOrGetById([FromRoute] string id)
        {
            var map = await _mapService.GetOrCreateAsync(id);

            if (map == null)
                return NotFound();

            var dto = map.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = map.Id }, dto);
        }

        [HttpPost("{id}/sync")]
        public async Task<ActionResult<MapDto>> SyncMap([FromRoute] string id)
        {
            var map = await _mapService.SyncMapAsync(id);

            if(map == null) 
                return NotFound();

            var dto = map.ToDto();
            
            return Ok(map.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            var result = await _mapService.DeleteByIdAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
