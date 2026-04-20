using RaidSense.Server.Dtos.RustMaps;
using RaidSense.Server.Interfaces.Services;

namespace RaidSense.Server.Services
{
    public class RustMapsApiService : IRustMapsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public RustMapsApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;   
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

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<RustMapsResponseDto>();

                return result?.Data;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
