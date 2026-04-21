using RaidSense.Server.Dtos.RustMaps;
using RaidSense.Server.Interfaces.Services;

namespace RaidSense.Server.Services
{
    public class RustMapsApiService : ExternalApiService, IRustMapsApiService
    {
        private readonly IConfiguration _config;
        public RustMapsApiService(HttpClient httpClient, IConfiguration config) : base(httpClient)
        {
            _config = config;

            var apiToken = _config["RustMaps:ApiToken"];
            if (!string.IsNullOrEmpty(apiToken))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiToken);
            }
        }
        public async Task<RustMapsDataDto?> GetRustMapDetailsAsync(string mapId)
        {
            var url = $"https://api.rustmaps.com/v4/maps/{mapId}";
            var response = await GetAsync<RustMapsResponseDto>(url);
            
            return response?.Data;
        }
    }
}
