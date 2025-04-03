namespace Inde;

public class McConfigurationSource : IConfigurationSource
{
    public Serilog.ILogger Logger { get; }
    public string? AuthorizationValue { get; set; }
    public string? ConfigurationUrl { get; set; }
    public string? InstanceId { get; set; }

    public McConfigurationSource(IConfiguration options, Serilog.ILogger logger)
    {
        Logger = logger;
        AuthorizationValue = options["Authorization"] ?? "UG9ydGFsU1U6UGFzc3cwcmQx";
        InstanceId = options["InstanceId"] ?? "1";
        var baseUrl = options["BaseUrl"] ?? "https://insight-qa.inntopia.com/IntegrationApi";
        ConfigurationUrl = $"{baseUrl}/v2/HostAdapter/configuration/adapterconfiguration";
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new McConfigurationProvider(this);
    }
}
