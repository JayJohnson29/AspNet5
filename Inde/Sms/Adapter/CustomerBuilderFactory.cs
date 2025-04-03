using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sms.Entity;

namespace Sms.Adapter;

public class CustomerBuilderFactory : ICustomerBuilderFactory
{
    private readonly ILogger<CustomerBuilderFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CustomerBuilderFactory(ILogger<CustomerBuilderFactory> logger, IServiceProvider serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
    }

    public ICustomerBuilder Create(Guest source, int clientId, int sourceSystemCode)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<CustomerBuilder>>();
        return new CustomerBuilder(logger, clientId, sourceSystemCode, source);
    }
}
