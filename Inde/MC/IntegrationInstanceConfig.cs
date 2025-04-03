namespace MC;

public class IntegrationInstanceConfig
{
    public int HostAdapterInstanceConfigId { get; set; }
    public int HostAdapterInstanceId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Boolean IsActive { get; set; }
    public string AdapterName { get; set; }
    public int ClientId { get; set; }
    public int SourceSystemCode { get; set; }
    public int ScheduleId { get; set; }
    public string ScheduleFunctions { get; set; }
    public int LookupCodeId { get; set; }
    public int UserKey { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UploadDate { get; set; }
}