using Microsoft.AspNetCore.Authorization;
using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Repositories;
using RaidSense.Server.Security.Handlers;
using RaidSense.Server.Services;

namespace RaidSense.Server.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRustServerRepository, RustServerRepository>();
            services.AddScoped<IMapRepository, MapRepository>();
            services.AddScoped<IMapAccessRepository, MapAccessRepository>();
            services.AddScoped<IUserMapRepository, UserMapRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IRustServerService, RustServerService>();
            services.AddScoped<IRustMapService, RustMapService>();
            services.AddScoped<IMapAccessService, MapAccessService>();
            services.AddScoped<IUserMapService, UserMapService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthorizationHandler, MapAccessHandler>();

            services.AddHttpClient<IBattlemetricsService, BattlemetricsService>();
            services.AddHttpClient<IRustMapsApiService, RustMapsApiService>();

            return services;
        }
    }
}
