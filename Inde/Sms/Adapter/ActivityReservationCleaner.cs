using Sms.Entity;
using System.Xml.Linq;
using System;

namespace Sms.Adapter;

public static class ActivityReservationCleaner
{
    public static ActivityReservation Clean(ActivityReservation source)
    {
        var destination = new ActivityReservation
        {
            sknum = source.sknum.Trim(),
            sktype = source.sktype.Trim(),
            sklev = source.sklev.Trim(),
            skfirst = source.skfirst.Trim(),
            sklast = source.sklast.Trim(),
            skgnum = source.skgnum.Trim(),
            skppl = source.skppl,
            skcnum = source.skcnum,
            skdate = source.skdate,
            sktime = source.sktime.Trim(),
            skroom = source.skroom.Trim(),
            SKDEPAMT = source.SKDEPAMT,
            skpromo = source.skpromo.Trim(),
            skprice = source.skprice,
            sknote = source.sknote.Trim(),
            skbill = source.skbill.Trim(),
            skposrcpt = source.skposrcpt.Trim(),
            skbkop = source.skbkop.Trim(),
            skbkdate = source.skbkdate,
            skbktime = source.skbktime.Trim(),
            sklink = source.sklink.Trim(),
            skcart = source.skcart.Trim(),
            skcaddy = source.skcaddy.Trim(),
            skcaddydbl = source.skcaddydbl.Trim(),
            skcaddyfc = source.skcaddyfc.Trim(),
            skclub1 = source.skclub1.Trim(),
            skclub2 = source.skclub2.Trim(),
            skvip = source.skvip.Trim(),
            skgrate = source.skgrate.Trim(),
            skchlog = source.skchlog.Trim(),
            skhole = source.skhole.Trim(),
            skpkg = source.skpkg.Trim(),
            skmrkt = source.skmrkt.Trim(),
            sksubmrkt = source.sksubmrkt.Trim(),
            skproduct = source.skproduct.Trim(),
            sksdur = source.sksdur,
            skrdur = source.skrdur,
            rrname = source.rrname.Trim(),
            rrholes = source.rrholes,
            roomTextDescription = source.roomTextDescription.Trim(),
            roomFacilityDescription = source.roomFacilityDescription.Trim(),
            marketDescription = source.marketDescription.Trim(),
            sksvc = source.sksvc.Trim(),
            rsname = source.rsname.Trim(),
            rsdesc = source.rsdesc.Trim(),
            skdepdue = source.skdepdue,
            golfRateCourseCode = source.golfRateCourseCode.Trim(),
            golfRateDescription = source.golfRateDescription.Trim(),
            golfRateType = source.golfRateType.Trim(),
            golfcancpol = source.golfcancpol.Trim(),
            productDescription = source.productDescription.Trim(),
        };

        return destination;
    }
}
