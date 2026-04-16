using RaidSense.Server.Dtos.BattleMetrics.Players;
using RaidSense.Server.Dtos.BattleMetrics.Server;
using RaidSense.Server.Interfaces.Services;
using RaidSense.Server.Mappers;
using RaidSense.Server.Models.Battlemetrics;
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

        public async Task<BmPlayerDto?> GetPlayerDetailsAsync(string playerId)
        {
            var url = $"https://api.battlemetrics.com/players/{playerId}";
            var response = await GetFromApiAsync<BmPlayerResponseDto>(url);
            throw new NotImplementedException();
        }

        public async Task<BmServerDto?> GetServerDetailsAsync(string serverId)
        {
            var url = $"https://api.battlemetrics.com/servers/{serverId}";
            var response = await GetFromApiAsync<BmServerResponseDto>(url);

            return response?.ToServerDto();
        }
        
        private async Task<TResponse?> GetFromApiAsync<TResponse>(string url) where TResponse : class
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TResponse>(url);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
