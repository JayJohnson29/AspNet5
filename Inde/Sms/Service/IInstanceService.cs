using MC;

namespace Sms.Service;

public interface IInstanceService
{
    Task ExecuteAsync(CancellationToken stoppingToken, IntegrationInstanceConfig integrationInstanceConfig);
}