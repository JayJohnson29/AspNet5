using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using Sms.MC;

namespace Sms.Adapter;

public class ItineraryBuilderFactory : IItineraryBuilderFactory
{
    private readonly ILogger<ItineraryBuilderFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ItineraryBuilderFactory(ILogger<ItineraryBuilderFactory> logger, IServiceProvider serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
    }

    public ItineraryBuilder Create(AppConfig config, LetterHistoryItinerary source)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<ItineraryBuilder>>();
        return new ItineraryBuilder(logger, config,source);
    }
}
