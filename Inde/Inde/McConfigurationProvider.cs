using System.Net.Http.Headers;
using System.Text.Json;

namespace Inde;

public class McConfigurationProvider : ConfigurationProvider
{
    public McConfigurationSource Source { get; }

    public McConfigurationProvider(McConfigurationSource source)
    {
        Source = source;
    }


    public override void Load()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Source.AuthorizationValue);
            client.DefaultRequestHeaders.Add("ClientId", "0");
            client.DefaultRequestHeaders.Add("UserId", "integrationApi");
            client.DefaultRequestHeaders.Add("HostAdapterInstanceId", Source.InstanceId);

            var url = $"{Source.ConfigurationUrl}";

            var response = client.GetAsync(new Uri(url)).Result;
            if (!response.IsSuccessStatusCode)
            {
                // handle error --- without mc configuration the api will not work -- perhaps throw exception
                return;
            }

            var returnValue = response.Content.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var apiResponse = JsonSerializer.Deserialize<Response<List<IntegrationInstanceConfiguration>>>(returnValue, options);

            if (apiResponse == null)
            {
                // handle error --- without mc configuration the api will not work-- perhaps throw exception
                return;
            }

            var configJson = JsonSerializer.Serialize(apiResponse.Data);
            Set($"MarketingCloud:Adapters", configJson);



            // todo verify required parameters -- for example Connection string is required
            //foreach (var item in apiResponse.Data)
            //{
            //    var configJson = JsonSerializer.Serialize(item);
            //    Source.Logger.Information($"Name: {item.InstanceConfig.Name} ");
            //    Set($"MarketingCloud:{item.InstanceConfig.Name}", configJson);

            //}
        }
    }
}
