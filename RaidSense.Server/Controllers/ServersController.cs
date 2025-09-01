using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.RustServer;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private IRustServerService _rustServerService;
        public ServersController(IRustServerService rustServerService)
        {
            _rustServerService = rustServerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RustServerDto>> GetById([FromRoute] string id)
        {
            var server = await _rustServerService.GetByIdAsync(id);

            if (server == null)
                return NotFound();

            var dto = server.ToDto();

            return Ok(dto);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<RustServerDto>> CreateOrGetById([FromRoute] string id)
        {
            var server = await _rustServerService.GetOrCreateAsync(id);

            if (server == null)
                return NotFound("Server not found or it doesnt support rustmaps");

            var dto = server.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = server.Id }, dto); 
        }

        [HttpGet]
        public async Task<ActionResult<List<RustServerDto>>> GetAll()
        {
            var servers = await _rustServerService.GetAllAsync();
            var dtos = servers.Select(s => s.ToDto()).ToList();

            return Ok(dtos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            var result = await _rustServerService.DeleteByIdAsync(id);

            return result ? NoContent() : NotFound();
        }

        [HttpPost("{id}/sync")]
        public async Task<ActionResult<RustServerDto>> SyncServer([FromRoute] string id)
        {
            var server = await _rustServerService.SyncServerAsync(id);

            if (server == null)
                return NotFound();

            var dto = server.ToDto();
            
            return Ok(dto);
        }
    }
}
