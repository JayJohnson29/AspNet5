using System.ComponentModel.DataAnnotations;

namespace Sms.Entity;

public class ItineraryArrival
{
    [Key]
    public int ItineraryArrivalId { get; set; }

    public int SmsIntegrationId { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    public int StatusId { get; set; }

    public string StatusMessage { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

}
