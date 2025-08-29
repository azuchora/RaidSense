using RaidSense.Server.Dtos.BattleMetrics.Server;
using RaidSense.Server.Interfaces;
using RaidSense.Server.Mappers;
using System.Net.Http.Headers;

namespace RaidSense.Server.Services
{
    public class BattlemetricsService : IBattlemetricsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public BattlemetricsService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            var apiToken = _config["BattleMetrics:ApiToken"];
            if(!string.IsNullOrEmpty(apiToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            }
        }

        public async Task<BmServerDto?> GetServerDetailsAsync(string serverId)
        {
            var url = $"https://api.battlemetrics.com/servers/{serverId}";
            BmServerResponse? response = null;
            try
            {

                response = await _httpClient.GetFromJsonAsync<BmServerResponse>(url);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            if (response?.Data?.Attributes?.Details?.RustMaps == null)
                return null;

            return response.ToServerDto();
        }
    }
}
