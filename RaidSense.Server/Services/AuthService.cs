using System;
using Microsoft.AspNetCore.Identity;
using RaidSense.Server.Constants;
using RaidSense.Server.Dtos.Auth;
using RaidSense.Server.Exceptions.Http;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models;

namespace RaidSense.Server.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserRepository _userRepo;
    private const string InvalidCredentials = "Invalid username or password";

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService,
        IUserRepository userRepo
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        _userRepo = userRepo;
    }
    
    public async Task<(AuthDto, RefreshToken)> LoginAsync(LoginDto dto, string ipAddress, string? refreshToken)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null)
            throw new UnauthorizedException(InvalidCredentials);
        
        var login = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (!login.Succeeded)
            throw new UnauthorizedException(InvalidCredentials);

        var accessToken = await _tokenService.CreateAccessTokenAsync(user);
        var newRefreshToken = await _refreshTokenService.RotateTokenAsync(user, refreshToken, ipAddress);
        
        return (user.ToAuthDto(accessToken), newRefreshToken);
    }

    public async Task LogoutAsync(string refreshToken, string ipAddress)
    {
        var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);

        if (user is not null)
            await _refreshTokenService.RevokeTokenAsync(user, refreshToken, ipAddress);
    }

    public async Task<(RefreshDto, RefreshToken)> RefreshAsync(string refreshToken, string ipAddress)
    {
        var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);

        if (user is null)
            throw new UnauthorizedException(InvalidCredentials);

        var token = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

        if (token is null || !token.IsActive)
            throw new UnauthorizedException(InvalidCredentials);

        var newAccessToken = await _tokenService.CreateAccessTokenAsync(user);
        var newRefreshToken = await _refreshTokenService.RotateTokenAsync(user, refreshToken, ipAddress);

        var dto = new RefreshDto
        {
            AccessToken = newAccessToken,
        };

        return (dto, newRefreshToken);
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        if (await _userManager.FindByNameAsync(dto.Username) != null)
            throw new ConflictException("Username is taken");

        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, Roles.User);
    }
}
