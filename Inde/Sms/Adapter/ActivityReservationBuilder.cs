using Microsoft.Extensions.Logging;
using Sms.Entity;
using MC;

namespace Sms.Adapter;

public class ActivityReservationBuilder : IActivityReservationBuilder
{
    private Reservation _reservation;
    private readonly ILogger<ActivityReservationBuilder> _logger;

    public Reservation Reservation { get { return _reservation; } }


    public ActivityReservationBuilder(ILogger<ActivityReservationBuilder> logger, AppConfig config, string cenRezId, ActivityReservation source)
    {
        _logger = logger;


        var sknum = source.sknum;  // dr.HAGetString("sknum");
        var sktype = source.sktype; //  dr.HAGetString("sktype");
        var skLevel = source.sklev; // dr.HAGetString("sklev");
        var skprice = source.skprice; // dr.HAGetFieldValue<decimal>("skprice");
        var arrivalDate = DateTimeUtil.CreateDateTime(source.skdate, source.sktime);
        var skppl = source.skppl; // dr.HAGetFieldValue<int>("skppl");
        var skgrate = source.skgrate; // dr.HAGetString("skgrate");
        var sksvc = source.sksvc; // dr.HAGetString("sksvc");
        var rsdesc = source.rsdesc; // dr.HAGetString("rsdesc");
        var rsname = source.rsname; // dr.HAGetString("rsname");
        var golfRateDescription = source.golfRateDescription; // dr.HAGetString("golfRateDescription");
        var golfRateType = source.golfRateType; // dr.HAGetString("GolfRateType");


        //// remove dining activity reservations
        //if (sktype.ToLower() == "d" && _currentInstance.CID == 224 && DateTime.Compare(DateTime.Now, new DateTime(2024, 4, 17)) > 0)
        //{
        //    return null;
        //}


        // set to empty for pb handlebar coder == if golf rate type == empty
        if (string.Equals(sktype, "G", StringComparison.CurrentCultureIgnoreCase))
        {
            golfRateType = string.IsNullOrEmpty(golfRateType) ? "Empty" : golfRateType;
        }
        var rrname = source.rrname; // dr.HAGetString("rrname");
        var skRoom = source.skroom; //  dr.GetString("skroom");
        var skProduct = source.skproduct; // dr.HAGetString("skproduct");
        var note = source.sknote; // dr.HAGetString("sknote");
        var roomFacilityDescription = source.roomFacilityDescription; // dr.HAGetString("roomFacilityDescription");
        var guestNum = source.skgnum; // dr.HAGetString("skgnum");
        var firstName = source.skfirst; // dr.HAGetString("skfirst");
        var lastName = source.sklast; //  dr.HAGetString("sklast");


        var perPersonRate = skprice;

        if (skppl > 0)
            perPersonRate = skprice / skppl;

        var scheduleType = "";
        var reservationType = ReservationType.Other;


        switch (sktype.ToLower())
        {
            case "i":
                scheduleType = "Spa";
                reservationType = ReservationType.Spa;
                break;
            case "c":
                scheduleType = "Class";
                reservationType = ReservationType.Class;
                break;
            case "g":
                scheduleType = "Golf";
                reservationType = ReservationType.Golf;
                break;
            case "d":
                scheduleType = "Dining";
                reservationType = ReservationType.FoodAndBeverage;
                break;
            default:
                scheduleType = sktype;
                reservationType = ReservationType.Other;
                break;
        }


        Reservation res = new Reservation
        {
            ReservationId = sknum, //  dr.HAGetString( "sknum");
            CenRezId = cenRezId,
            CID = config.ClientId, //  Constants.CLIENT_ID,
            SID = config.SourceSystemCode,
            Type = reservationType,
            BookingChannel = ReservationBookingChannel.PMS,
            Version = "1.4", // _currentInstance.SchemaVersion.ToString(),
            UpdateDate = DateTime.Now,
            LodgingFlag = false,
            Status = GetScheduleStatus(skLevel),
            AgentId = source.skbkop,// dr.HAGetString("skbkop"),
            AgentName = string.Empty,
            BookingDate = DateTimeUtil.CreateDateTime(source.skbkdate.Value, source.skbktime),
            ArrivalDate = arrivalDate,
            DepartureDate = arrivalDate,
            NumberOfDays = 1,
            IATANumber = string.Empty,
            NumberOfAdults = skppl.ToString(),
            NumberOfChildren = "0",
            ResendValue = string.Empty,
            SendConfirmation = false,
            Totals = new ReservationTotals(),
            MarketSegment = new ReservationMarketSegment
            {
                Code = source.skmrkt, // dr.HAGetString("skmrkt"),
                Description = source.marketDescription, // dr.HAGetString("marketDescription"),
                Group = string.Empty,
            },
        }; 

        if (!string.IsNullOrEmpty(note))
        {
            res.Notes = new ReservationNote[]
            {
                    new ReservationNote
                    {
                        UserId = source.skbkop, // dr.HAGetString("skbkop"),
                        ChangeType = "",
                        Comment = note,
                        NoteKey = 0,
                        NoteCreateDate = DateTime.Now,
                        NoteUpdateDate = DateTime.Now,
                        SourceSystemKey = source.skgnum, // dr.HAGetString("skgnum"),
                    },
            };
        }

        res.RatePlan = new ReservationRatePlan
        {
            Code = string.IsNullOrEmpty(skgrate) ? sksvc : skgrate,
            Description = string.IsNullOrEmpty(skgrate) ? rsdesc : golfRateDescription,
            Group = "NG",
        };




        var reservationNameValues = new List<ReservationNameValue>
            {
                new ReservationNameValue { Name = "Level", Value = skLevel },
                new ReservationNameValue { Name = "SkDate", Value = $"{source.skdate:d} 12:00:00 AM" },    //  dr.HAGetString("skdate"), },
                new ReservationNameValue { Name = "SkTime", Value =  source.sktime }, // dr.HAGetString("sktime"), },
                new ReservationNameValue { Name = "ArrivalDateShort", Value = $"{res.ArrivalDate:d}", },
                new ReservationNameValue { Name = "ArrivalDateLong", Value = $"{res.ArrivalDate:D}", },
                new ReservationNameValue { Name = "ArrivalDateTime", Value = $"{res.ArrivalDate:h:mm tt}", },
                new ReservationNameValue { Name = "DepartureDateShort", Value = $"{res.DepartureDate:d}", },
                new ReservationNameValue { Name = "DepartureDateLong", Value = $"{res.DepartureDate:D}", },
                new ReservationNameValue { Name = "DepositDueDate", Value = $"{source.skdepdue:yyyy-MM-dd}" }, // dr.HAGetDateTimeString("skdepdue", "yyyy-MM-dd"), },
                new ReservationNameValue { Name = "Price", Value = $"{skprice:N}" },
                new ReservationNameValue { Name = "DepositAmount", Value =  $"{source.SKDEPAMT:N}" },// dr.HAGetFieldValueAsString<decimal>("skdepamt", "N") },
                new ReservationNameValue { Name = "PerPersonRate", Value = $"{perPersonRate:N}" },
                new ReservationNameValue { Name = "ServiceCode", Value = sksvc },
                new ReservationNameValue { Name = "ServiceDescription", Value = rsdesc },
                new ReservationNameValue { Name = "ServiceName", Value = rsname },
                new ReservationNameValue { Name = "GolfRateCode", Value = skgrate },
                new ReservationNameValue { Name = "GolfRateDescription", Value =golfRateDescription },
                new ReservationNameValue { Name = "GolfRateCourseCode", Value = source.golfRateCourseCode },  // dr.HAGetString("golfRateCourseCode") },
                new ReservationNameValue { Name = "GolfRateType", Value = golfRateType },
                new ReservationNameValue { Name = "RoomName", Value = rrname },
                new ReservationNameValue { Name = "RoomTextDescription", Value = source.roomTextDescription }, // dr.HAGetString("roomTextDescription") },
                new ReservationNameValue { Name = "RoomFacilityDescription", Value = roomFacilityDescription  },
                new ReservationNameValue { Name = "NumberOfHoles", Value = $"{source.rrholes:0}"}, // dr.HAGetString("rrholes") },
                new ReservationNameValue { Name = "CaddySingle", Value = source.skcaddy },// dr.HAGetString("skcaddy") },
                new ReservationNameValue { Name = "CaddyDouble", Value = source.skcaddydbl },// dr.HAGetString("skcaddydbl") },
                new ReservationNameValue { Name = "CaddyFC", Value = source.skcaddyfc }, // dr.HAGetString("skcaddyfc") },
                new ReservationNameValue { Name = "FirstName", Value = firstName },
                new ReservationNameValue { Name = "LastName", Value = lastName },

                // fields from lodging reservations that are needed for confirmation processing (cid = 199)
                new ReservationNameValue { Name = "UnitTypeDescription", Value = string.Empty },
                new ReservationNameValue { Name = "UnitType", Value = string.Empty },
                new ReservationNameValue { Name = "RoomNumber", Value = string.Empty },
                new ReservationNameValue { Name = "SubmarketCode", Value = string.Empty },
                new ReservationNameValue { Name = "GuaranteeCode", Value = string.Empty },
                new ReservationNameValue { Name = "NumberOfPeople", Value = skppl.ToString() },
            };

        res.NameValues = reservationNameValues.ToArray();


        //if (!string.IsNullOrEmpty(guestNum))
        //{
        //    var guestResult = GetCustomerById(guestNum, _currentInstance.ConnectionString, _currentInstance.TimeoutSetting, _currentInstance.CID, _currentInstance.SID);
        //    res.Customer = guestResult.Item1;
        //}
        //else
        //{
        ////    res.Customer = BlankCustomer();
        ////    res.Customer.FirstName = firstName;
        ////    res.Customer.LastName = lastName;
        //}

        res.SourceSystem = new ReservationSourceSystem
        {
            GuestKey = source.skgnum, // dr.HAGetString("skgnum"),
            Id = config.SourceSystemCode,
            Name = "SMS",
        };

        var activity = new ReservationActivity
        {
            Amount = skprice.Value,
            ActivityId = res.ReservationId,
            Currency = config.CurrencyCode,
            Date = res.ArrivalDate,
            Duration = source.sksdur.Value.ToString(), // dr.HAGetString("sksdur"),
            Participants = new Customer[] { res.Customer },
            Status = GetActivityStatus(res.Status),
            Quantity = 1,
        };

        var eventLocationCode = skRoom;
        var eventLocationDescription = scheduleType;
        var eventLocationType = scheduleType;
        if (sktype.ToLower() == "g")
        {
            eventLocationCode = skRoom;
            eventLocationDescription = roomFacilityDescription;
            eventLocationType = "Golf";
        }
        else if (sktype.ToLower() == "d")
        {
            eventLocationCode = sksvc;
            eventLocationDescription = rsdesc;
            eventLocationType = "Dining";
        }
        else if (sktype.ToLower() == "c")
        {
            eventLocationCode = !string.IsNullOrEmpty(skRoom) ? skRoom : sksvc;
            eventLocationDescription = !string.IsNullOrEmpty(skRoom) ? roomFacilityDescription : rsname;
            eventLocationType = "Class";
        }
        else if (sktype.ToLower() == "i")
        {
            eventLocationCode = !string.IsNullOrEmpty(skRoom) ? skRoom : sksvc;
            eventLocationDescription = roomFacilityDescription;
            eventLocationType = "Class";
        }

        activity.EventLocation = new ReservationActivityEventLocation
        {
            Code = eventLocationCode,
            Description = eventLocationDescription,
            Resort = config.ResortName, // _currentInstance.Resort,
            ResortDetail = config.ResortName, // _currentInstance.Resort,
            Type = eventLocationType,
        };

        activity.Product = new ReservationActivityProduct
        {
            Code = !string.IsNullOrEmpty(skProduct) ? skProduct : sksvc,
            Description = !string.IsNullOrEmpty(skProduct) ? source.productDescription : rsdesc,   // dr.HAGetString("productDescription") : rsdesc,
            LOB = scheduleType,
            LOBSummary = scheduleType,
        };

        activity.PurchaseLocation = new ReservationActivityPurchaseLocation
        {
            Code = sksvc,
            Description = config.ResortName,
            Resort = config.ResortName,
            ResortDetail = config.ResortName,
            Type = "Resort",
        };


        var nameValues = new List<ReservationActivityNameValue>
            {
                //new ReservationActivityNameValue { Name = "Number of Holes", Value = dr.HAGetString("rrholes"), },
                new ReservationActivityNameValue { Name = "Activity Duration", Value = source.sksdur.ToString() }, // dr.HAGetString("sksdur"), },
                new ReservationActivityNameValue { Name = "ActivityDuration", Value = source.sksdur.ToString() },// dr.HAGetString("sksdur"), },
                new ReservationActivityNameValue { Name = "ActivityDateShort", Value = $"{activity.Date:d}", },
                new ReservationActivityNameValue { Name = "ActivityDateLong", Value = $"{activity.Date:D}", },
                new ReservationActivityNameValue { Name = "ActivityDateTime", Value = $"{activity.Date:h:mm tt}", },
                new ReservationActivityNameValue { Name = "ActivityServiceDescription", Value = rsdesc },
                new ReservationActivityNameValue { Name = "ActivityServiceName", Value = rsname },
                new ReservationActivityNameValue { Name = "ActivityNumberOfPeople", Value = skppl.ToString() },
            };
        activity.NameValues = nameValues.ToArray();

        res.Activities = new ReservationActivity[] { activity };
        _reservation = res;
    }


    //private DateTime GetBookingDate(DateTime res, string tm)
    //{
    //    //var res = dr.HAGetDateTime("skbkdate");

    //    //var tm = dr.HAGetString("skbktime");

    //    var x = new string[] { "00", "00" };
    //    if (tm.Contains(":"))
    //    {
    //        x = tm.Split(':');
    //    }

    //    var hours = 0;
    //    Int32.TryParse(x[0], out hours);
    //    hours = (hours < 0 || hours > 23) ? 0 : hours;


    //    var mins = 0;
    //    Int32.TryParse(x[1], out mins);
    //    mins = mins < 0 || mins > 59 ? 0 : mins;

    //    return new DateTime(res.Year, res.Month, res.Day, hours, mins, 0);
    //}

    private ReservationStatus GetScheduleStatus(string smsLevel)
    {
        switch (smsLevel.ToLower())
        {
            case "new":
                return ReservationStatus.New;
            case "cnf":
                return ReservationStatus.New;
            case "can":
                return ReservationStatus.Cancelled;
            case "out":
                return ReservationStatus.Completed;
            case "inh":
                return ReservationStatus.Completed;
            case "cla":
                return ReservationStatus.Completed;
            case "cvt":
                return ReservationStatus.Converted;
            default:
                return ReservationStatus.Completed;
        }
    }
    private ReservationActivityStatus GetActivityStatus(ReservationStatus resStatus)
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


    public void SetCustomer( Customer customer )
    {
        _reservation.Customer = customer;
        foreach (var a in _reservation.Activities)
        {
            a.Participants = [customer]; 
        }
    }

}
