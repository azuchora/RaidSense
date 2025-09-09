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
        public UserMapsController(IUserMapService userMapService)
        {
            _userMapService = userMapService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMapDto>>> GetMaps()
        {
            var userId = User.GetId();
            var dtos = await _userMapService.GetAllDtosByOwnerAsync(userId);
            return Ok(dtos);
        }
    }
}
