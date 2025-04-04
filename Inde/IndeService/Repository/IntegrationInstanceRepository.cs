using System.Text.Json;

namespace IndeService.Repository;

public class IntegrationInstanceRepository(ILogger<IntegrationInstanceRepository> logger, HttpClient httpClient)
    : IIntegrationInstanceRepository
{
    public async Task<List<IntegrationInstanceConfiguration>> GetAsync()
    {
        try
        {

            var response = await httpClient.GetAsync(httpClient.BaseAddress);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error getting integrations");
                return new List<IntegrationInstanceConfiguration>();
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
            logger.LogError(e, "Error retrieving integrations");
            return new List<IntegrationInstanceConfiguration>();
        }

    }
}

