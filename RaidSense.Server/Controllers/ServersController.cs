using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.Servers;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Controllers
{
    [Route("api/servers")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly IRustServerService _rustServerService;
        public ServersController(IRustServerService rustServerService)
        {
            _rustServerService = rustServerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerDto>> GetById([FromRoute] string id)
        {
            var server = await _rustServerService.GetByIdAsync(id);

            return Ok(server.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerDto>> EnsureExists([FromRoute] string id)
        {
            var server = await _rustServerService.EnsureExistsAsync(id);

            return CreatedAtAction(nameof(GetById), new { id = server.Id }, server.ToDto()); 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerDto>>> GetAll()
        {
            var servers = await _rustServerService.GetAllAsync();
            var dtos = servers.Select(s => s.ToDto());

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            await _rustServerService.DeleteByIdAsync(id);

            return NoContent();
        }

        [HttpPost("{id}/sync")]
        public async Task<ActionResult<ServerDto>> SyncServer([FromRoute] string id)
        {
            var server = await _rustServerService.SyncServerAsync(id);
            
            return Ok(server.ToDto());
        }
    }
}
