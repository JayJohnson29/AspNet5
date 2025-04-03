using MC;
using Microsoft.Extensions.Logging;

namespace Sms.Service;
public class InstanceService(ILogger<InstanceService> logger, ILetterService letterService, IItineraryArrivalService itineraryArrivalService,IDatabaseRestoreService databaseRestoreService)
    : IInstanceService
{

    public async Task<bool> ExecuteAsync(CancellationToken stoppingToken, IntegrationInstanceConfig integrationInstanceConfig)
    {

        try
        {

            var appConfig = new AppConfig
            {
                ClientId = integrationInstanceConfig.ClientId,
                SourceSystemCode = integrationInstanceConfig.SourceSystemCode,
                CurrencyCode = "USD",
                PropertyName = "pebble",
                ResortName = "pebble",
                SmsIntegrationId = -1, //result.Item2,
                HostPlusPath = $"C:\\temp\\smsdata\\Source\\HostPlus",
                ConnectionString = $"data source=192.168.1.2;database=pb_springermiller;user id=sa;password=tiger123$;Connection Timeout=120;Encrypt=False;",
            };

            var result = await databaseRestoreService.RunAsync(appConfig);
            if (!result.Item1)
            {
                logger.LogError("Rebuild database failed");
                return true;
            }

            appConfig.SmsIntegrationId = result.Item2;

            if (string.Equals(integrationInstanceConfig.ScheduleFunctions, "LetterRequests", StringComparison.InvariantCultureIgnoreCase))
            {
                var x = await letterService.RunAsync(stoppingToken, appConfig);
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        finally
        {
        }

    }
}

