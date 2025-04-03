using System.ComponentModel.DataAnnotations;

namespace Sms.Entity;

public class ItineraryHistory
{
    [Key]
    public int ItineraryHistoryId { get; set; }

    public int SmsIntegrationId { get; set; }

    public string icode { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int StatusId { get; set; }

    public string StatusMessage { get; set; }

}
