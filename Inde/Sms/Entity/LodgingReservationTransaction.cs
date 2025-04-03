namespace Sms.Entity;

public class LodgingReservationTransaction
{
    public string tnum { get; set; }
    public DateTime tdate { get; set; }
    public string tcode { get; set; }
    public decimal tdebit { get; set; }
    public decimal tcredit { get; set; }
    public decimal tqty { get; set; }
    public string tunit { get; set; }
    public string top { get; set; }

    public string cdescr { get; set; }


}
