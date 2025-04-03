using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sms.Entity;

namespace Sms.Adapter;

public class LodgingReservationBuilderFactory : ILodgingReservationBuilderFactory
{
    private readonly ILogger<LodgingReservationBuilderFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LodgingReservationBuilderFactory(ILogger<LodgingReservationBuilderFactory> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public LodgingReservationBuilder Create(LodgingReservation source, AppConfig config,  string cenRezId)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<LodgingReservationBuilder>>();

        return new LodgingReservationBuilder(logger,config, source, cenRezId);
    }
}
