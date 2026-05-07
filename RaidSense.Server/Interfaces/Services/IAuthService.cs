using System;
using RaidSense.Server.Dtos.Auth;
using RaidSense.Server.Models;

namespace RaidSense.Server.Interfaces.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task<(AuthDto, RefreshToken)> LoginAsync(LoginDto dto, string ipAddress, string? refreshToken);
    Task<(RefreshDto, RefreshToken)> RefreshAsync(string refreshToken, string ipAddress);
    Task LogoutAsync(string refreshToken, string ipAddress);
}
