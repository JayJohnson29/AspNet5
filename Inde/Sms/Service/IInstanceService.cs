using MC;

namespace Sms.Service;

public interface IInstanceService
{
    Task<Inde.Response<IterationExtract>> ExecuteAsync(CancellationToken stoppingToken, IntegrationInstanceConfig integrationInstanceConfig);
}