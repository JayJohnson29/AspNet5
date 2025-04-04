using MC;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace Sms.Service;
public class InstanceService(ILogger<InstanceService> logger, ILetterService letterService, IItineraryArrivalService itineraryArrivalService,IDatabaseRestoreService databaseRestoreService)
    : IInstanceService
{

    public async Task<Inde.Response<IterationExtract>> ExecuteAsync(CancellationToken stoppingToken, IntegrationInstanceConfig integrationInstanceConfig)
    {

        try
        {

            logger.LogInformation("SMS Instance Service");

            var appConfig = new AppConfig
            {
                ClientId = integrationInstanceConfig.ClientId,
                SourceSystemCode = integrationInstanceConfig.SourceSystemCode,
                CurrencyCode = "USD",
                PropertyName = "pebble",
                ResortName = "pebble",
                SmsIntegrationId = -1,
                HostPlusPath = $"C:\\temp\\smsdata\\Source\\HostPlus",
                //ConnectionString = $"data source=192.168.1.2;database=pb_springermiller;user id=sa;password=tiger123$;Connection Timeout=120;Encrypt=False;",
                ConnectionString = $"data source=localhost\\SQLEXPRESS;database=pb_springermiller;user id=pbdbr;password=tiger123$;integrated security=false;Connection Timeout=120;Encrypt=False;",
            };

            var result = await databaseRestoreService.RunAsync(appConfig);
            if (!result.Item1)
            {
                logger.LogError("Rebuild database failed");
                return new Inde.Response<IterationExtract> { Data = new IterationExtract(), Message = "rebuild database failed", IsSuccess = false };
            }

            appConfig.SmsIntegrationId =  result.Item2;

            if (string.Equals(integrationInstanceConfig.ScheduleFunctions, "LetterRequests", StringComparison.InvariantCultureIgnoreCase))
            {
                var letterRequestItineraries = await letterService.RunAsync(stoppingToken, appConfig);
                if (letterRequestItineraries.NumberOfRecords > 0)
                {
                    var xmlString = Inde.XmlService.SerializeObject<Itineraries>(letterRequestItineraries);
                    var doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                     
                    var fileName = $"{DateTime.Now:yyyyMMddHHmmss.fff}.{integrationInstanceConfig.SourceSystemCode}.letterrequest.SMS.V1.4.Xml";
                    return new Inde.Response<IterationExtract>{Data = new IterationExtract{ ExtractTypeId = 1,ExtractName = fileName, ExtractData = doc}, Message = "success", IsSuccess = true};
                }
            }
            else if (string.Equals(integrationInstanceConfig.ScheduleFunctions, "ItinerarArrivals", StringComparison.InvariantCultureIgnoreCase))
            {
                var itineraryArrivals = await itineraryArrivalService.Run(stoppingToken,appConfig);

                if (itineraryArrivals.NumberOfRecords > 0)
                {
                    var xmlString = Inde.XmlService.SerializeObject<Reservations>(itineraryArrivals);
                    var doc = new XmlDocument();
                    doc.LoadXml(xmlString);

                    var fileName = $"{DateTime.Now:yyyyMMddHHmmss.fff}.{appConfig.SourceSystemCode}.reservations.SMS.V1.4.Xml";
                    return new Inde.Response<IterationExtract> { Data = new IterationExtract { ExtractTypeId = 2, ExtractName = fileName, ExtractData = doc }, Message = "success", IsSuccess = true };
                }

            }


            return new Inde.Response<IterationExtract> { Data = new IterationExtract(), Message = "Unknown function", IsSuccess = false };

        }
        catch (Exception e)
        {
            return new Inde.Response<IterationExtract> { Data = new IterationExtract(), Message = "Error running Instance Extract database failed", IsSuccess = false };
        }


    }
}

