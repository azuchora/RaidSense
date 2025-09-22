using RaidSense.Server.Interfaces.Repositories;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Repositories;
using RaidSense.Server.Services;

namespace RaidSense.Server.Extensions
{
    public static class AppServicesExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRustServerRepository, RustServerRepository>();
            services.AddScoped<IMapRepository, MapRepository>();
            services.AddScoped<IMapUserRepository, MapUserRepository>();
            services.AddScoped<IUserMapRepository, UserMapRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
        }

        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IRustServerService, RustServerService>();
            services.AddScoped<IRustMapService, RustMapService>();
            services.AddScoped<IMapUserService, MapUserService>();
            services.AddScoped<IUserMapService, UserMapService>();
            services.AddScoped<IBaseService, BaseService>();

            services.AddHttpClient<IBattlemetricsService, BattlemetricsService>();
            services.AddHttpClient<IRustMapsApiService, RustMapsApiService>();
        }
    }
}
