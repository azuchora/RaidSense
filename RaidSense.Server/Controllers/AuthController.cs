using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Constants;
using RaidSense.Server.Dtos.Auth;
using RaidSense.Server.Interfaces;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models;

namespace RaidSense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(ITokenService tokenService,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userManager.FindByNameAsync(registerDto.Username) != null)
                return Conflict("Username is taken");

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, Roles.User);

            var token = await _tokenService.CreateTokenAsync(user);

            var authDto = user.ToAuthDto(token);

            return Ok(authDto); // todo createdat
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var login = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!login.Succeeded)
                return Unauthorized("Invalid username or password");

            var token = await _tokenService.CreateTokenAsync(user);

            var authDto = user.ToAuthDto(token);

            return Ok(authDto);
        }
    }
}
