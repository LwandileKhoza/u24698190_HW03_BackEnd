namespace FilmLog.API.Services
{
    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://www.omdbapi.com";
        private readonly string _apiKey;

        public OmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Omdb:ApiKey"]!;
        }

        public async Task<string> SearchByTitle(string title)
        {
            try
            {
                var url = $"{_baseUrl}/?t={Uri.EscapeDataString(title)}&apikey={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling OMDB API: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
