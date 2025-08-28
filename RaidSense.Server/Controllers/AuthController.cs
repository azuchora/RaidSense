using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            if (Request.Cookies.TryGetValue("refreshToken", out var existingTokenValue))
            {
                var oldToken = user.RefreshTokens
                    .SingleOrDefault(t => t.Token == existingTokenValue && t.IsActive);

                if (oldToken != null)
                {
                    oldToken.Revoked = DateTime.UtcNow;
                    oldToken.RevokedByIp = ipAddress;
                }
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken(ipAddress);

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

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
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var newRefreshToken = _tokenService.GenerateRefreshToken(ipAddress);

            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            token.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

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
                {
                    var token = user.RefreshTokens.Single(t => t.Token == refreshTokenValue);
                    token.Revoked = DateTime.UtcNow;
                    token.RevokedByIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                    await _userManager.UpdateAsync(user);
                }

                _tokenService.DeleteRefreshTokenCookie(Response);
            }

            return NoContent();
        }
    }
}
