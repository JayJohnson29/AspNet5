using System.Text.Json;

namespace IndeService.Repository;

public class IntegrationInstanceRepository : IIntegrationInstanceRepository
{
    private readonly ILogger<IntegrationInstanceRepository> _logger;
    private readonly HttpClient _httpClient;

    public IntegrationInstanceRepository(ILogger<IntegrationInstanceRepository> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<List<IntegrationInstanceConfiguration>> GetAsync()
    {
        try
        {
           // _httpClient.DefaultRequestHeaders.Add("UserToken", token);

            var response = await _httpClient.GetAsync(_httpClient.BaseAddress);

            if (!response.IsSuccessStatusCode)
            {
                //Log.Error($"Error in Post StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase} ");
                //return new Response<T> { Message = "Error in Http Post", Data = default(T), Success = false };
                return null;
            }

            var responseText = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var responseValue = JsonSerializer.Deserialize<Response<List<IntegrationInstanceConfiguration>>>(responseText,options);
            return responseValue.Data;
        }
        catch (Exception e)
        {
            return new List<IntegrationInstanceConfiguration>();
        }

    }
}

