using Inde;

public static class McConfigurationProviderExtension
{
    public static IConfigurationBuilder AddMcConfiguration(this IConfigurationBuilder configuration, IConfigurationSection options, Serilog.ILogger logger)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        configuration.Add(new McConfigurationSource(options, logger));
        return configuration;
    }
}
