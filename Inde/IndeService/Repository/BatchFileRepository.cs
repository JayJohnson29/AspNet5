using Serilog;
using System.Net.Http.Headers;

namespace IndeService.Repository;
public class BatchFileRepository(ILogger<BatchFileRepository> logger, HttpClient httpClient) : IBatchFileRepository
{
    public async Task<Response<string>> SendLetterRequestAsync(string body)
    {

        httpClient.DefaultRequestHeaders.Add("SMSLetterRequest", "true");

        var content = new StringContent(body);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var uri = $"{httpClient.BaseAddress}/v2/ImportItineraryXmlFiles";

        var response = await httpClient.PostAsync(uri, content);

        if (!response.IsSuccessStatusCode)
        {
            Log.Error($"Error in Post StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase} ");
            return new Response<string> { Message = "Error in Http Post", Data = "Error posting itinerary file", Success = false };
        }

        var responseText = await response.Content.ReadAsStringAsync();


        return new Response<string> { Data = string.Empty, Success = true, Message = string.Empty };
    }
    public async Task<Response<string>> SendXmlFile(string body)
    {

        var content = new StringContent(body);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var uri = $"{httpClient.BaseAddress}/v1/ImportXmlFiles";

        var response = await httpClient.PostAsync(uri, content);

        if (!response.IsSuccessStatusCode)
        {
            Log.Error($"Error in Post StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase} ");
            return new Response<string> { Message = "Error in Http Post", Data = "Error posting itinerary file", Success = false };
        }

        var responseText = await response.Content.ReadAsStringAsync();


        return new Response<string> { Data = string.Empty, Success = true, Message = string.Empty };
    }

}
