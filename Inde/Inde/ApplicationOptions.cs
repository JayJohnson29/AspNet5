namespace Inde;

public class ApplicationOptions
{

    public static ConfigurationSettings GetConfigurationSettings(IConfiguration config)
    {
        return new ConfigurationSettings
        {
            InstanceId = 1,
            BaseUrl = "https:\\\\insightqa.inntopia.com",
            UserName = "JJohnson",
            Password = "RedSeven2",
        };

    }
}
