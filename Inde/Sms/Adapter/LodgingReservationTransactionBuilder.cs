using Microsoft.Extensions.Logging;
using Sms.Entity;
using MC;

namespace Sms.Adapter;

public class LodgingReservationTransactionBuilder
{
    private readonly ILogger<LodgingReservationTransactionBuilder> _logger;
    private List<ReservationActivity> _reservationActivityList;

    public List<ReservationActivity> RservationActivities { get { return _reservationActivityList; } }

    public LodgingReservationTransactionBuilder(ILogger<LodgingReservationTransactionBuilder> logger, AppConfig config, Reservation reservation, List<LodgingReservationTransaction> transactions)
    {
        _logger = logger;
        _reservationActivityList = new List<ReservationActivity>();

        var reservationId = reservation.ReservationId;
        int nights = reservation.NumberOfDays;

        //string currencyCode = config.CurrencyCode;

        ReservationStatus resStatus = reservation.Status;
        DateTime arrivalDate = reservation.ArrivalDate;
        decimal totalAfterTax = reservation.Totals.AfterTax;

        var nv = reservation.NameValues.ToList();
        var unitsNv = nv.FirstOrDefault(n => string.Equals(n.Name, "units", StringComparison.OrdinalIgnoreCase));
        var inResUnits = 0;
        if (unitsNv != null)
        {
            int.TryParse(unitsNv.Value, out inResUnits);
        }
        string unit = string.Empty;
        var unitNv = nv.FirstOrDefault(n => string.Equals(n.Name, "RoomNumber", StringComparison.OrdinalIgnoreCase));
        if (unitNv != null)
        {
            unit = unitNv.Value;
        }

        var eventLocation = GetEventLocation(unit, config.ResortName, config.PropertyName);

        var purchaseLocation = GetPurchaseLocation(unit, config.ResortName, config.PropertyName);


        foreach (var t in transactions)
        {

            _reservationActivityList.Add(new ReservationActivity
            {
                ActivityId = reservationId,
                Amount = decimal.Round(t.tdebit / nights, 2),
                Currency = config.CurrencyCode,
                Date = t.tdate,
                Quantity = (int)t.tqty,
                Status = GetActivityStatus(resStatus),
                EventLocation = eventLocation,
                Product = new ReservationActivityProduct
                {
                    Code = string.IsNullOrEmpty(t.tcode) ? "RySol Generic Lodge" : t.tcode,
                    Description = string.IsNullOrEmpty(t.cdescr) ? "Generic Lodging Product" : t.cdescr,
                    LOB = "Lodging",
                    LOBSummary = "Lodging",
                },
                PurchaseLocation = purchaseLocation,


                NameValues =
                        [
                            new ReservationActivityNameValue { Name = "PostedUser", Value = t.top, },
                            new ReservationActivityNameValue { Name = "ActivityDateShort", Value = $"{t.tdate:d}" },
                            new ReservationActivityNameValue { Name = "ActivityDateLong", Value = $"{t.tdate:D}" },
                            new ReservationActivityNameValue { Name = "ActivityDateTime", Value = $"{t.tdate:h:mm tt}" },
                        ],
            });

        }

        if (resStatus == ReservationStatus.InHouse || resStatus == ReservationStatus.Completed && transactions.Count == 0)
        {
            var genericActivities = GetGenericActivity(arrivalDate, totalAfterTax, inResUnits, unit, nights, reservationId, resStatus, config.CurrencyCode, eventLocation, purchaseLocation);
            _reservationActivityList = genericActivities;
        }
        else
        {
            var genericActivities = GetGenericActivity(arrivalDate, totalAfterTax, inResUnits, unit, nights, reservationId, resStatus, config.CurrencyCode, eventLocation, purchaseLocation);
            if (genericActivities.Count > 0)
                _reservationActivityList.AddRange(genericActivities);
        }

    }

    public List<ReservationActivity> GetGenericActivity(DateTime eventDate, decimal amount, int units, string unit, int nights, string resId, ReservationStatus resStatus, string currencyCode,
    ReservationActivityEventLocation eventLocation,
    ReservationActivityPurchaseLocation purchaseLocation)
    {

        List<ReservationActivity> activities = new List<ReservationActivity>();
        for (int i = 1; i <= nights; i++)
        {

            activities.Add(new ReservationActivity
            {
                ActivityId = resId,
                Amount = decimal.Round(amount / nights, 2),
                Currency = currencyCode,
                Date = eventDate,
                Quantity = (int)units,
                Status = GetActivityStatus(resStatus),
                EventLocation = eventLocation,
                Product = new ReservationActivityProduct
                {
                    Code = "RySol Generic Lodge",
                    Description = "Generic Lodging Product",
                    LOB = "Lodging",
                    LOBSummary = "Lodging",
                },
                PurchaseLocation = purchaseLocation,
                NameValues = [
                                new ReservationActivityNameValue { Name = "PostedUser", Value = string.Empty, },
                                new ReservationActivityNameValue { Name = "ActivityDateShort", Value = $"{eventDate.Date:d}" },
                                new ReservationActivityNameValue { Name = "ActivityDateLong", Value = $"{eventDate.Date:D}" },
                                new ReservationActivityNameValue { Name = "ActivityDateTime", Value = $"{eventDate.Date:h:mm tt}" },
                ],

            });

        }
        return activities;
    }


    public ReservationActivityPurchaseLocation GetPurchaseLocation(string unit, string resort, string propertyName)
    {

        if (string.IsNullOrEmpty(unit))
        {
            return new ReservationActivityPurchaseLocation();
        }

        unit = unit.Trim();

        ReservationActivityPurchaseLocation purchLoc = new ReservationActivityPurchaseLocation
        {
            Code = "RySolGeneric1",
            Description = "Central Reservations",
            Resort = resort,
            Type = "Reservations",
            ResortDetail = propertyName
        };

        return purchLoc;
    }

    public ReservationActivityEventLocation GetEventLocation(string unit, string resort, string propertyName)
    {

        if (string.IsNullOrEmpty(unit))
        {
            return new ReservationActivityEventLocation();
        }

        return new ReservationActivityEventLocation
        {
            Code = unit,
            Description = "Unit: " + unit,
            Resort = resort,
            Type = "Lodging",
            ResortDetail = propertyName,
        };

    }


    //private ReservationActivityProduct GetProduct(string productCode)
    //{
    //    //_log.Debug($"Begin: GetProduct({productCode})");

    //    if (string.IsNullOrEmpty(productCode))
    //    {
    //        return new ReservationActivityProduct();
    //    }

    //    var command = $"select cdescr,* from {_dbName}.dbo.in_codes where ccode = '{productCode}'";

    //    SqlDataReader dr = null;

    //    ReservationActivityProduct product = null;

    //    using (SqlConnection conn = new SqlConnection(_currentInstance.ConnectionString))
    //    {
    //        using (SqlCommand oCmd = new SqlCommand(command, conn))
    //        {
    //            conn.Open();
    //            oCmd.CommandType = CommandType.Text;
    //            dr = oCmd.ExecuteReader();

    //            while (dr.Read())
    //            {
    //                product = new ReservationActivityProduct
    //                {
    //                    Code = productCode,
    //                    Description = dr.HAGetString(0),
    //                    LOB = "Unknown",
    //                    LOBSummary = "Unknown"
    //                };

    //            }
    //        }
    //    }

    //    //if the product description contains 'system use' then dont add it
    //    if ((product?.Description?.ToLower().IndexOf("system") >= 0) && (product?.Description?.ToLower().IndexOf("use") > 0))
    //    {
    //        return null;
    //    }

    //    if (product == null)
    //    {
    //        return GetSpecialProducts(productCode);
    //    }


    //    // _log.Debug("End: GetProduct(string productCode)");
    //    return product;
    //}

    public ReservationActivityProduct GetSpecialProducts(string productCode)
    {
        if ((!string.IsNullOrEmpty(productCode)) && productCode.Length >= 3 && productCode.Substring(0, 3).ToLower() == "rru")
        {
            return new ReservationActivityProduct
            {
                Code = productCode,
                Description = "Room Upgrade",
                LOB = "Unknown",
                LOBSummary = "Unknown"
            };
        }

        return null;
    }

    public ReservationActivityStatus GetActivityStatus(ReservationStatus resStatus)
    {
        switch (resStatus)
        {
            case ReservationStatus.Completed:
                return ReservationActivityStatus.Completed;
            case ReservationStatus.New:
                return ReservationActivityStatus.New;
            case ReservationStatus.Converted:
                return ReservationActivityStatus.Converted;
            case ReservationStatus.Cancelled:
                return ReservationActivityStatus.Cancelled;
            default:
                return ReservationActivityStatus.Completed;
        }
    }
}
