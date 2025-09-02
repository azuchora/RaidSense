using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Interfaces.Services;

namespace RaidSense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMapsController : ControllerBase
    {
        private readonly IUserMapService _userMapService;
        public UserMapsController(IUserMapService userMapService)
        {
            _userMapService = userMapService;
        }
    }
}
