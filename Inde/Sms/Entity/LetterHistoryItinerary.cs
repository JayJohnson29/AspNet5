namespace Sms.Entity;

public class LetterHistoryItinerary
{
    public int LetterHistoryItineraryId { get; set; }

    public int letterhistoryid { get; set; }

    public string lnum { get; set; }

    public string lcode { get; set; }

    public string lguestnum { get; set; }

    public string lmod { get; set; }

    public string lacctnum { get; set; }

    public string icode { get; set; }

    public string iguest { get; set; }

    public DateTime icdate { get; set; } = new DateTime(1900,1,1);
       
    public string ictime { get; set; }

    public DateTime iexpdate { get; set; } = new DateTime(1900, 1, 1);

    public string iexptime { get; set; }

    public DateTime iarrive { get; set; } = new DateTime(1900, 1, 1);

    public DateTime idepart { get; set; } = new DateTime(1900, 1, 1);

    public string ibkdprop { get; set; }

    public decimal icharges { get; set; } = 0m;

    public decimal ipayments { get; set; } = 0m;

    public string iconfnum { get; set; }

}
