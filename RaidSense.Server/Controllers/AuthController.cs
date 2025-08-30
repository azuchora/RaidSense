using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Constants;
using RaidSense.Server.Dtos.Auth;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
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
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            ITokenService tokenService,
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IRefreshTokenService refreshTokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
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

            return Created();
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

            var accessToken = await _tokenService.CreateAccessTokenAsync(user);
            string ipAddress = HttpContext.GetIpAddress();
            Request.Cookies.TryGetValue("refreshToken", out var existingTokenValue);
            var newRefreshToken = await _refreshTokenService.RotateTokenAsync(user, existingTokenValue, ipAddress);
            _tokenService.SetRefreshTokenCookie(Response, newRefreshToken);

            var authDto = user.ToAuthDto(accessToken);

            return Ok(authDto);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshDto>> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized("Refresh token is missing");

            var user = _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null) 
                return Unauthorized("Invalid refresh token");

            var token = user.RefreshTokens.Single(x => x.Token == refreshToken);

            if (!token.IsActive) 
                return Unauthorized("Invalid or expired refresh token");

            var newAccessToken = await _tokenService.CreateAccessTokenAsync(user);
            string ipAddress = HttpContext.GetIpAddress();
            var newRefreshToken = await _refreshTokenService.RotateTokenAsync(user, refreshToken, ipAddress);

            _tokenService.SetRefreshTokenCookie(Response, newRefreshToken);

            var response = new RefreshDto { AccessToken = newAccessToken };
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshTokenValue))
            {
                var user = _userManager.Users
                    .Include(u => u.RefreshTokens)
                    .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshTokenValue));

                if (user != null)
                    await _refreshTokenService.RevokeTokenAsync(user, refreshTokenValue, HttpContext.GetIpAddress());

                _tokenService.DeleteRefreshTokenCookie(Response);
            }
            
            return NoContent();
        }
    }
}
