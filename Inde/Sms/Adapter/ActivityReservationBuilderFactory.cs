using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sms.Entity;

namespace Sms.Adapter;

public class ActivityReservationBuilderFactory : IActivityReservationBuilderFactory
{
    private readonly ILogger<ActivityReservationBuilderFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ActivityReservationBuilderFactory(ILogger<ActivityReservationBuilderFactory> logger, IServiceProvider serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
    }

    public ActivityReservationBuilder Create(AppConfig config, string itineraryId, ActivityReservation source)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<ActivityReservationBuilder>>();
        return new ActivityReservationBuilder(logger, config, itineraryId, source);
    }
}
