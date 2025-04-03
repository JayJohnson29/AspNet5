

using MC;

namespace IndeService;
public class IntegrationInstanceConfiguration
{
    public IntegrationInstanceConfig InstanceConfig { get; set; }
    public Schedule Schedule { get; set; }
    public string FtpUserName { get; set; }
    public string FtpPassword { get; set; }
}
