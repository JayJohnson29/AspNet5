using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sms.Entity;

public class ActivityReservation
{
    public string sknum { get; set; }

    public string sktype { get; set; }

    public string sklev { get; set; }

    public string skfirst { get; set; }

    public string sklast { get; set; }

    public string skgnum { get; set; }

    public decimal? skppl { get; set; }

    public decimal? skcnum { get; set; }

    public DateTime skdate { get; set; }

    public string sktime { get; set; }

    public string skroom { get; set; }

    public decimal? SKDEPAMT { get; set; }

    public string skpromo { get; set; }

    public decimal? skprice { get; set; }

    public string sknote { get; set; }

    public string skbill { get; set; }

    public string skposrcpt { get; set; }

    public string skbkop { get; set; }

    public DateTime? skbkdate { get; set; }

    public string skbktime { get; set; }

    public string sklink { get; set; }

    public string skcart { get; set; }

    public string skcaddy { get; set; }

    public string skcaddydbl { get; set; }

    public string skcaddyfc { get; set; }

    public string skclub1 { get; set; }

    public string skclub2 { get; set; }

    public string skvip { get; set; }

    public string skgrate { get; set; }

    public string skchlog { get; set; }

    public string skhole { get; set; }

    public string skpkg { get; set; }

    public string skmrkt { get; set; }

    public string sksubmrkt { get; set; }

    public string skproduct { get; set; }

    public decimal? sksdur { get; set; }

    public decimal? skrdur { get; set; }

    public string rrname { get; set; }

    public decimal? rrholes { get; set; }

    public string roomTextDescription { get; set; }

    public string roomFacilityDescription { get; set; }

    public string marketDescription { get; set; }

    public string sksvc { get; set; }

    public string rsname { get; set; }

    public string rsdesc { get; set; }

    public DateTime? skdepdue { get; set; }

    public string golfRateCourseCode { get; set; }

    public string golfRateDescription { get; set; }

    public string golfRateType { get; set; }

    public string golfcancpol { get; set; }

    public string productDescription { get; set; }

}
