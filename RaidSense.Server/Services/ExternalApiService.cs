namespace RaidSense.Server.Services;

public class ExternalApiService
{
    protected readonly HttpClient _httpClient;

    protected ExternalApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<T?> GetAsync<T>(string url) where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            if(!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
