using System.Net.Http.Headers;
using System.Text.Json;

namespace IndeService.Repository;
public class McLoginRepository : IMcLoginRepository
{
    private readonly ILogger<McLoginRepository> _logger;
    private readonly HttpClient _httpClient;
   // private readonly IConfiguration _configuration;
   private readonly string _userName;
   private readonly string _password;

    public McLoginRepository(ILogger<McLoginRepository> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        //_configuration = configuration;
        var val = configuration["MarketingCloud:UserName"];
        _userName = val ?? string.Empty;
        val = configuration["MarketingCloud:Password"];
        _password = val ?? string.Empty;
    }

    public async Task<McLoginResponse> Login()
    {

        var request = new McLoginRequest
        {
            UserName = _userName,
            Password = _password,
        };

        var body = JsonSerializer.Serialize(request);

        var content = new StringContent(body);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

        if (!response.IsSuccessStatusCode)
        {
            //Log.Error($"Error in Post StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase} ");
            //return new Response<T> { Message = "Error in Http Post", Data = default(T), IsSuccess = false };
            return null;
        }

        var responseText = await response.Content.ReadAsStringAsync();

        var loginResponse = JsonSerializer.Deserialize<McLoginResponse>(responseText);
        return loginResponse;
    }

}

