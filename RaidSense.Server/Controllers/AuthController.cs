using Microsoft.AspNetCore.Mvc;
using RaidSense.Server.Dtos.Auth;
using RaidSense.Server.Exceptions.Http;
using RaidSense.Server.Extensions;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Models;

namespace RaidSense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);
            return Created();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginDto loginDto)
        {
            var (authDto, newRefreshToken) = await _authService.LoginAsync(
                loginDto,
                GetClientIp(),
                GetRefreshToken()
            );

            SetRefreshToken(newRefreshToken);

            return Ok(authDto);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshDto>> Refresh()
        {   
            var refreshToken = GetRefreshTokenOrThrow();

            var (refreshDto, newRefreshToken) = await _authService.RefreshAsync(
                refreshToken,
                GetClientIp()
            );

            SetRefreshToken(newRefreshToken);

            return Ok(refreshDto);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = GetRefreshToken();

            if(refreshToken is not null)
            {
                await _authService.LogoutAsync(
                    refreshToken,
                    GetClientIp()
                );
            }

            DeleteRefreshToken();

            return NoContent();
        }

        private string GetClientIp()
            => HttpContext.GetIpAddress();
        
        private string? GetRefreshToken()
        {
            Request.Cookies.TryGetValue(
                "refreshToken",
                out var token
            );

            return token;
        }

        private string GetRefreshTokenOrThrow()
        {
            return GetRefreshToken()
                ?? throw new UnauthorizedException("Missing refresh token");
        }

        private void SetRefreshToken(RefreshToken token)
        => _tokenService.SetRefreshTokenCookie(Response, token);

        private void DeleteRefreshToken()
            => _tokenService.DeleteRefreshTokenCookie(Response);
        }
}
