using RaidSense.Server.Dtos.RustMaps;
using RaidSense.Server.Interfaces.Services;
using System.Net.Http.Headers;

namespace RaidSense.Server.Services
{
    public class RustMapsService : IRustMapsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public RustMapsService(HttpClient httpClient, IConfiguration config)
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
            RustMapsResponseDto? response = null;
            try
            {
                var res = await _httpClient.GetAsync(url);
                
                response = await _httpClient.GetFromJsonAsync<RustMapsResponseDto>(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
            return response?.Data;
        }
    }
}
