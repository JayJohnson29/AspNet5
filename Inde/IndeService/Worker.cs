using IndeService.Repository;
using IndeService.Service;

namespace IndeService
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
                var a = await _integrationService.RunAsync(stoppingToken);
                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
