namespace Sms.Entity;

public class SmsIntegration
{
    public int SmsIntegrationId { get; set; }

    public DateTime SmsIntegrationStartTime { get; set; }

    public DateTime SmsIntegrationEndTime { get; set; }

    public int StatusId { get; set; }

    public string Comment { get; set; }

    public DateTime StageUpdateBeginDate { get; set; }

    public DateTime StageUpdateEndDate { get; set; }


}
