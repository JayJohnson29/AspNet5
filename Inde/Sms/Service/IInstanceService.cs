using MC;

namespace Sms.Service;

public interface IInstanceService
{
    Task<bool> ExecuteAsync(CancellationToken stoppingToken, IntegrationInstanceConfig integrationInstanceConfig);
}