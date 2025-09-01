using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Constants;
using RaidSense.Server.Dtos.RustServer;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;

namespace RaidSense.Server.Controllers
{
    [Route("api/servers")]
    [ApiController]
    public class RustServerController : ControllerBase
    {
        private IRustServerService _rustServerService;
        public RustServerController(IRustServerService rustServerService)
        {
            _rustServerService = rustServerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RustServerDto>> GetOrCreateServer([FromRoute] string id)
        {
            var server = await _rustServerService.GetOrCreateAsync(id);

            if (server == null)
                return NotFound("Server not found or it doesnt support rustmaps");

            return Ok(server.ToDto());
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

            return server == null ? NoContent() : Ok(server.ToDto());
        }
    }
}
