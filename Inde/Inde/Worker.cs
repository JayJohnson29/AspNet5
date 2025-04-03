using Inde.Repository;
using Inde.Service;

namespace Inde
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IIntegrationService _integrationService;

        public Worker(ILogger<Worker> logger, IIntegrationService integrationService)
        {
            _logger = logger;
            _integrationService = integrationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Wow Man");
                }
                var a = await _integrationService.RunAsync(stoppingToken);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
