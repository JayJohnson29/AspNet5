using Microsoft.Extensions.Logging;
using Serilog;
using Sms.Entity;
using Sms.MC;

namespace Sms.Adapter;

public class LodgingReservationBuilder : ILodgingReservationBuilder
{

    private Reservation reservation;
    private List<ReservationNameValue> nameValueList;
    private List<ReservationReservationRequest> reservationRequests;
    private readonly ILogger<LodgingReservationBuilder> _logger;
    private readonly List<ReservationActivity> _genericActivities;
    private readonly ReservationActivityEventLocation _eventLocation;
    private readonly ReservationActivityPurchaseLocation _purchaseLocation;

    private const string InnAtSpanishBay = "The Inn at Spanish Bay";
    private const string LodgeAtPebbleBeach = "The Lodge at Pebble Beach";
    private const string CasaPalmero = "Casa Palmero";

    private const string GolfSpanishBay = "Spanish Bay";
    private const string GolfPebbleBeach = "Pebble Beach";
    private const string GolfSpyglass = "SpyGlass";
    private const string GolfTheHay = "The Hay";
    private const string GolfDelMonte = "Del Monte";


    public Reservation Reservation
    {
        get
        {
            reservation.NameValues = nameValueList.ToArray();
            reservation.ReservationRequests = reservationRequests.ToArray();
            return reservation;
        }
    }


    public LodgingReservationBuilder(ILogger<LodgingReservationBuilder> logger, AppConfig config, LodgingReservation source, string cenRezId)
    {
        _logger = logger;
        reservation = new Reservation();
        nameValueList = [];
        reservationRequests = [];

        var anum = source.anum;
        var level = source.level;
        var unit = source.unit;
        var unitrbase = source.unitrtbase;
        var agentCode = source.agent;
        var inResUnits = Convert.ToInt32(source.units);
        var inResTotal = source.total;
        var inResExtras = source.extras;
        var inResPayments = source.payments;
        var rsource = source.rsource;   //   dr.HAGetString("rsource");
        var package = source.package;   //  dr.HAGetString("package");
        var market = source.mrkt;       //  dr.HAGetString("mrkt");
        var rcancpol = source.rcancpol; // dr.HAGetString("rcancpol");
        var rdep1pol = source.rdep1pol; // dr.HAGetString("rdep1pol");
        var rdep2pol = source.rdep2pol; // dr.HAGetString("rdep2pol");
        var featrs = source.featrs;     // dr.HAGetString("featrs");
        var special = source.Special;   // dr.HAGetString("Special");
        var cancellationId = string.Equals(source.level,"CAN",StringComparison.InvariantCultureIgnoreCase) ? GetCancellationId(source.resno, source.CancelDate) : string.Empty;
        var vipStatus = string.Empty;
        var housekeepingNotes = source.hnotes; // dr.HAGetString("hnotes");
        var auditNotes = source.anotes; //  dr.HAGetString("anotes");
        var additionalEmailRecipient = string.Empty;
        var housekeepingEmailValue = string.Empty;
        var share = source.share; //  dr.HAGetString("share");

        reservation.CID = config.ClientId; //  Constants.CLIENT_ID;
        reservation.SID = config.SourceSystemCode;
        reservation.Type = ReservationType.Lodging;
        reservation.BookingChannel = ReservationBookingChannel.PMS;
        reservation.Version = "1.4";
        reservation.LodgingFlag = true;
        reservation.ReservationId = source.resno;
        reservation.CenRezId = cenRezId;
        reservation.ArrivalDate = source.arrival;
        reservation.DepartureDate = source.depart;
        reservation.NumberOfAdults = source.anum.ToString();
        reservation.NumberOfChildren = source.cnum.ToString();
        reservation.IATANumber = source.agent;
        reservation.BookingDate = source.booking;
        reservation.AgentId = source.lastuser;
        reservation.AgentName = source.lastuser;
        reservation.UpdateDate = source.CurrentUpdateDate;
        reservation.Status = GetReservationStatus(source.level);
        //reservation.ResendValue = source.dialed;

        reservation.SourceSystem = new ReservationSourceSystem { GuestKey = source.guestNum, Id = config.SourceSystemCode, Name = "SMS" };

        reservation.NumberOfDays = (int)source.nights;   // dr.HAGetFieldValue<int>("nights");
        if (reservation.NumberOfDays == 0)
        {
            reservation.NumberOfDays = 1;
        }

        decimal adr = source.subtotal / reservation.NumberOfDays;
        reservation.Totals = new ReservationTotals
        {
            Discount = source.disc, // dr.HAGetFieldValue<decimal>("disc"),
            DiscountSpecified = true,
            BeforeTax = source.subtotal,  //dr.HAGetFieldValue<decimal>("subtotal"),
            BeforeTaxSpecified = true,
            Tax = source.taxes, // dr.HAGetFieldValue<decimal>("taxes"),
            TaxSpecified = true,
            AfterTax = inResTotal,
            AfterTaxSpecified = true,
            CurrencyCode = config.CurrencyCode,
            TotalRooms = inResUnits,
            DepositReceived = inResPayments,
            RoomRate = source.subtotal, // dr.HAGetFieldValue<decimal>("subtotal"),
            Other = inResExtras,
            OtherSpecified = true,
            Fees = source.grat, // dr.HAGetFieldValue<decimal>("grat"),
            BalanceDue = source.balance, // dr.HAGetFieldValue<decimal>("balance"),
        };

        reservation.Totals.ADR = decimal.Round(reservation.Totals.RoomRate / reservation.NumberOfDays, 2);

        var perPersonRate = reservation.Totals.AfterTax / anum;


        reservation.RatePlan = new ReservationRatePlan
        {
            Code = package,
            Description = source.rdesc, // dr.HAGetString("rdesc"),
            Group = "Unknown",
        };

        nameValueList =
        [
                    new() { Name = "RoomNumber", Value = unit },
                    new() { Name = "BookingUserId", Value =  source.op },                     // dr.HAGetString("op") },
                    new() { Name = "GroupCode", Value = source.group },                       //   dr.HAGetString("group") },
                    new() { Name = "ReservationGuestName", Value = source.ReservationName }, // dr.HAGetString("ReservationName"), },
                    new() { Name = "GuaranteeCode", Value = source.guar }, // dr.HAGetString("guar"), },
                    new() { Name = "SuppressRate", Value =  source.suprate }, // dr.HAGetString("suprate"), },
                    new() { Name = "OtherInParty", Value = $"{source.onum:0}" }, //   dr.HAGetString("onum") },
                    new() { Name = "LastEdit", Value = $"{source.lastedit:d} 12:00:00 AM" }, // dr.HAGetString("LastEdit") },
                    new() { Name = "SecretaryCode", Value = source.RSECY }, // dr.HAGetString("RSECY") },
                    new() { Name = "ExternalOrderId", Value = source.ConfNum }, //  dr.HAGetString("ConfNum") },
                    new() { Name = "ExternalSource", Value = source.ConfSystem }, // dr.HAGetString("ConfSystem") },
                    new() { Name = "ArrivalTime", Value = source.ESTARR }, // dr.HAGetString("ESTARR"), },
                    new() { Name = "CheckOutTime", Value = source.CHOUTTIME }, // dr.HAGetString("CHOUTTIME"), },
                    new() { Name = "PriceBasis", Value = unitrbase },
                    new() { Name = "UnitRTBase", Value = unitrbase, },
                    new() { Name = "ReservationNotes", Value = source.rnotes }, //     // Regex.Replace(dr.HAGetString("rnotes"), @"[^\u0020-\u00FF]", " ") },
                    new() { Name = "AuditNotes", Value = auditNotes },
                    new() { Name = "HousekeepingNotes", Value = housekeepingNotes },
                    new() { Name = "MealCode", Value = source.meal }, // dr.HAGetString("meal") },
                    new() { Name = "CorporationCode", Value = source.rcorp }, // dr.HAGetString("rcorp") },
                    new() { Name = "SubmarketCode", Value = source.submarket }, // dr.HAGetString("submarket") },
                    new() { Name = "Level", Value = level },
                    new() { Name = "DepositDue1", Value = $"{source.depdue:yyyy-MM-dd}" }, // dr.HAGetDateTimeString("depdue", "yyyy-MM-dd") },
                    new() { Name = "DepositDue2", Value = $"{source.depdue2:yyyy-MM-dd}"   }, // dr.HAGetDateTimeString("depdue2", "yyyy-MM-dd") },
                    new() { Name = "DepositAmount1", Value = $"{source.depamt:F}"  }, // dr.HAGetFieldValueAsString<decimal>("depamt", "F") },
                    new() { Name = "DepositAmount2", Value = $"{source.depamt2:F}"  }, // dr.HAGetFieldValueAsString<decimal>("depamt2", "F") },
                    new() { Name = "CancellationDate", Value =  $"{source.CancelDate:yyyy-MM-dd}"  }, // dr.HAGetDateTimeString("CancelDate", "yyyy-MM-dd") },
                    new() { Name = "PhoneIn", Value = source.dialed }, // dr.HAGetString("dialed") },
                    new() { Name = "PaymentType", Value = source.ptyp }, // dr.HAGetString("ptyp") },
                    new() { Name = "Special", Value = source.Special }, // dr.HAGetString("special") },
                    new() { Name = "SharePay", Value = source.shrinh }, // dr.HAGetString("shrinh") },
                    new() { Name = "UpgradeCode", Value = source.upgrade }, // dr.HAGetString("upgrade") },
                    new() { Name = "GroupTotal", Value = $"{source.grptotal:F}"  }, // dr.HAGetFieldValueAsString<decimal>("grptotal", "F") },
                    new() { Name = "CorporateTotal", Value = $"{source.crptotal:F}"   }, // dr.HAGetFieldValueAsString<decimal>("crptotal", "F") },
                    new() { Name = "ArrivalDateShort", Value = $"{reservation.ArrivalDate:d}", },
                    new() { Name = "ArrivalDateLong", Value = $"{reservation.ArrivalDate:D}", },
                    new() { Name = "ArrivalDateTime", Value = $"{reservation.ArrivalDate:h:mm tt}", },
                    new() { Name = "DepartureDateShort", Value = $"{reservation.DepartureDate:d}", },
                    new() { Name = "DepartureDateLong", Value = $"{reservation.DepartureDate:D}", },
                    new() { Name = "RateNotes", Value = source.rtext }, // dr.HAGetString("rtext") },
                    new() { Name = "RateType", Value = source.ratetypr }, //  dr.HAGetString("ratetypr") },
                    new() { Name = "RateFeatures", Value = source.rfeatrs }, // dr.HAGetString("rfeatrs") },
                    new() { Name = "RateWebNotes", Value = source.RTWEBTEXT }, // dr.HAGetString("RTWEBTEXT") },
                    new() { Name = "rcancpol", Value = rcancpol },
                    new() { Name = "rdep1pol", Value = rdep1pol },
                    new() { Name = "rdep2pol", Value = rdep2pol },
                    new() { Name = "PerPersonRate", Value = $"{perPersonRate:N}"},
                    new() { Name = "VIPStatus", Value = vipStatus, },
                    new() { Name = "AvgDailyRate", Value = $"{adr:N}" },
                    new() { Name = "CancellationId", Value = cancellationId },
                    new() { Name = "Share", Value = share },
                    new() { Name = "Units", Value =  $"{inResUnits:N}" },
                    new() { Name = "GroupContact", Value =  source.grpcontact },
                    new() { Name = "GroupName", Value =  source.grpname },
                    new() { Name = "GroupLocation", Value = $"{source.grpcity} {source.grpstate}" },
                    new() { Name = "GroupRank", Value =  source.grprank },
                    new() { Name = "ReservationContactPhone", Value =  source.contactphonenum },
                    new() { Name = "ReservationContact", Value =  source.contactphonedescr },
                    new() { Name = "AgencyName", Value =  source.taname },
                    new() { Name = "AgencyContact", Value =  source.tacontact },
                    //new() { Name = "AgencyLocation", Value =  $"{source.tacity}, {source.tastate}" },
            ];

        var agentLocation = string.Empty;
        if (!string.IsNullOrEmpty(source.tacity) && !string.IsNullOrEmpty(source.tastate))
        {
            agentLocation = $"{source.tacity}, {source.tastate}";
        }
        else if (string.IsNullOrEmpty(source.tastate) && !string.IsNullOrEmpty(source.tastate))
        {
            agentLocation = $"{source.tastate}";
        }
        nameValueList.Add(new ReservationNameValue { Name = "AgencyLocation", Value = agentLocation });


        // Check Housekeeping notes field for PB
        var copyIndex = housekeepingNotes.IndexOf("copy:", StringComparison.CurrentCultureIgnoreCase);
        if (copyIndex == -1 && housekeepingNotes.Contains("@") && housekeepingNotes.Contains("."))  //  no copy: and potential email addr
        {
            housekeepingEmailValue = housekeepingNotes;
        }
        else if (!string.IsNullOrEmpty(housekeepingNotes) && copyIndex != -1)  // copy found 
        {
            additionalEmailRecipient = housekeepingNotes.Substring(copyIndex + 5);  // +5 remove  "copy:"
        }

        nameValueList.Add(new ReservationNameValue { Name = "AdditionalEMailRecipients", Value = additionalEmailRecipient });
        nameValueList.Add(new ReservationNameValue { Name = "HousekeepingEmailOverride", Value = housekeepingEmailValue });

        _eventLocation = GetEventLocation(unit, config.ResortName, config.PropertyName);
        _purchaseLocation = GetPurchaseLocation(unit, config.ResortName, config.PropertyName);

        if (config.ClientId == 224)
        {
            var result = GetPropertyName(unit);
            if (result.Item1)
            {
                _eventLocation.ResortDetail = result.Item2;
                _purchaseLocation.ResortDetail = result.Item2;
            }
        }


        _genericActivities = GetGenericActivity(source.arrival, inResTotal, inResUnits, unit, reservation.NumberOfDays, reservation.ReservationId, reservation.Status, config.CurrencyCode, _eventLocation, _purchaseLocation);

    }

    public void SetUnit(List<LodgingUnit> unitDescriptions, string unit)
    {
        nameValueList.AddRange(GetUnit(unitDescriptions, unit));
    }

    public List<ReservationNameValue> GetUnit(List<LodgingUnit> unitDescriptions, string unit)
    {

        // check for match based on unum, if no match check tycod

        var nameValues = new List<ReservationNameValue>
        {
            new ReservationNameValue { Name = "UnitType", Value = string.Empty },
            new ReservationNameValue { Name = "UnitTypeDescription", Value = string.Empty},
            new ReservationNameValue { Name = "UnitRating", Value = string.Empty},
            new ReservationNameValue { Name = "UnitTypeLongDescription", Value = string.Empty},
            new ReservationNameValue { Name = "UnitName", Value =string.Empty},
        };

        try
        {
            var urating = string.Empty;
            var uname = string.Empty;

            if (unitDescriptions.Count() == 0 || string.IsNullOrEmpty(unit))
            {
                return nameValues;
            }

            var description = unitDescriptions.FirstOrDefault(d => string.Equals(d.unum, unit, StringComparison.OrdinalIgnoreCase));

            if (description != null)
            {
                return
                [
                    new ReservationNameValue { Name = "UnitType", Value = description.tycod },
                    new ReservationNameValue { Name = "UnitTypeDescription", Value =description.tydes},
                    new ReservationNameValue { Name = "UnitRating", Value = description.urating},
                    new ReservationNameValue { Name = "UnitTypeLongDescription", Value = description.tylong},
                    new ReservationNameValue { Name = "UnitName", Value =description.uname },
                ];
            }

            description = unitDescriptions.FirstOrDefault(d => string.Equals(d.tycod, unit, StringComparison.OrdinalIgnoreCase));

            if (description != null)
            { 
                // if unit is a type code then set rating and unit name to blank, since they are not in the units table
                return
                [
                    new ReservationNameValue { Name = "UnitType", Value = description.tycod },
                    new ReservationNameValue { Name = "UnitTypeDescription", Value =description.tydes},
                    new ReservationNameValue { Name = "UnitRating", Value = string.Empty},
                    new ReservationNameValue { Name = "UnitTypeLongDescription", Value = description.tylong},
                    new ReservationNameValue { Name = "UnitName", Value =string.Empty },
                ];
            }


            return nameValues;

        }
        catch (Exception e )
        {

            return nameValues;
        }
    }

    public void SetPriceBasis(List<LodgingUnit> unitDescriptions, string unit)
    {
        nameValueList.AddRange(GetPriceBasis(unitDescriptions, unit));
    }
    public List<ReservationNameValue> GetPriceBasis(List<LodgingUnit> unitDescriptions, string unit)
    {
        // seach based on tycod

        var nameValues = new List<ReservationNameValue>
        {
            new ReservationNameValue { Name = "PriceBasisDescription", Value = "Empty" },
            new ReservationNameValue { Name = "UnitRTBaseDescription", Value = string.Empty },
        };

        try
        {
            if (unitDescriptions.Count() == 0 || string.IsNullOrEmpty(unit))
            {
                return nameValues;
            }


            // match on unit number first -- then try unit type
            var description = unitDescriptions.FirstOrDefault(d => string.Equals(d.unum, unit, StringComparison.OrdinalIgnoreCase));

            if (description == null)
            {
                description = unitDescriptions.FirstOrDefault(d => string.Equals(d.tycod, unit, StringComparison.OrdinalIgnoreCase));
            }

            // no match on unit or type
            if (description == null)
            {
                return nameValues;
            }

            return new List<ReservationNameValue>
            {
                new() { Name = "PriceBasisDescription", Value = string.IsNullOrEmpty(description.tydes) ? "Empty" : description.tydes },
                new() { Name = "UnitRTBaseDescription", Value = description.tydes },
            };

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error loading price basis information for unit {unit}", unit);
            return nameValues;
        }
    }

    public void SetReservationRequests(List<Entity.Misc> miscDescriptions, string featrs)
    {

        if (!string.IsNullOrEmpty(featrs) && miscDescriptions.Count > 0)
        {

            // featrs is a pipe delimited string
            var features = featrs.Split('|');
            for (int i = 1; i <= features.Length; i++)
            {
                var code = features[i - 1].Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    var featureValue = miscDescriptions.FirstOrDefault(x => x.MiscTypeId == 2 && x.File == "F" && x.Code == code);
                    var description = featureValue == null ? string.Empty : featureValue.Description;
                    reservationRequests.Add(new ReservationReservationRequest { RequestCode = code, RequestType = "FEATURE", RequestText = description });
                }
            }
        }

    }

    public void SetReservationSpecialBillingRequests(List<Entity.Misc> miscDescriptions, List<Entity.LodgingReservationSpecialBilling> specialBillingCodes, string special)
    {

        if (!string.IsNullOrEmpty(special) && miscDescriptions.Count > 0)
        {
            for (int i = 0; i < special.Length; i++)
            {
                var code = special[i];
                if (!char.IsWhiteSpace(code))
                {
                    var codeString = code.ToString();

                    var specialValue = miscDescriptions.FirstOrDefault(x => x.MiscTypeId == 1 && x.File == "S" && x.Code == codeString);
                    var description = specialValue == null ? "Unknown" : specialValue.Description;
                    reservationRequests.Add(new ReservationReservationRequest { RequestCode = codeString, RequestType = "SPECIAL", RequestText = description });
                }
            }
        }

        var spc = specialBillingCodes.Select(s => new ReservationReservationRequest { RequestCode = s.scod, RequestText = s.sdes, RequestType = !string.IsNullOrEmpty(s.styp) ? s.styp : "SPECIALBILLING" }).ToList();

        if ( spc.Count > 0)
        {
            reservationRequests.AddRange(spc);
        }
    }


    public void SetRatePlanPolicies(List<LodgingReservationRatePlanPolicy> policies, string code, string name)
    {
        nameValueList.AddRange(GetRatePlanPolicies(policies, code, name));
    }

    public List<ReservationNameValue> GetRatePlanPolicies(List<LodgingReservationRatePlanPolicy> policies, string name, string code)
    {
        var policyTextList = new List<ReservationNameValue>
            {
                new ReservationNameValue { Name = $"{name}Text1", Value = string.Empty },
                new ReservationNameValue { Name = $"{name}Text2", Value = string.Empty },
                new ReservationNameValue { Name = $"{name}Text3", Value = string.Empty },
                new ReservationNameValue { Name = $"{name}Text4", Value = string.Empty },
            };

        try
        {

            if (string.IsNullOrEmpty(code) || policies.Count == 0)
            {
                return policyTextList;
            }


            var a = policies.FirstOrDefault(p => p.plcod == code);
            if (a == null)
            {
                return policyTextList;
            }

            return new List<ReservationNameValue>
                        {
                            new ReservationNameValue {Name = $"{name}Text1", Value = a.pltxt1},
                            new ReservationNameValue {Name = $"{name}Text2", Value = a.pltxt2},
                            new ReservationNameValue {Name = $"{name}Text3", Value = a.pltxt3},
                            new ReservationNameValue {Name = $"{name}Text4", Value = a.pltxt4},
                        };
        }
        catch (Exception ex)
        {
            return policyTextList;
        }
    }

    public void SetLastEditUser(List<User> users, string code)
    {

        var userName = GetUserName(users, code);

        nameValueList.Add(new ReservationNameValue { Name = "LastEditUser", Value = userName });
    }

    public void SetBookingUser(List<User> users, string code)
    {
        var userName = GetUserName(users, code);

        nameValueList.Add(new ReservationNameValue { Name = "BookingUser", Value = userName });
    }


    public string GetUserName(List<User> users, string code)
    {
        var user = users.FirstOrDefault(u => string.Equals(u.UsrCode, code, StringComparison.OrdinalIgnoreCase));
        return user?.UsrName ?? string.Empty;

    }

    public void SetMarketSegment(List<Misc> miscDescriptions, string code)
    {
        var description = string.Empty;

        var a = miscDescriptions.FirstOrDefault(d => d.Code == code && d.MiscTypeId == 1 && string.Equals(d.File, "M", StringComparison.OrdinalIgnoreCase));
        if (a != null)
            description = a.Description;

        reservation.MarketSegment = new ReservationMarketSegment { Code = code, Description = description, Group = "Unknown" };

    }
    public void SetSourceOfBusiness(List<SourceOfBusiness> sobList, string rsource)
    {
        var a = sobList.FirstOrDefault(s => s.scode == rsource);
        var description = a?.sdescrip ?? string.Empty;


        reservation.SourceOfBusiness = new ReservationSourceOfBusiness { Code = rsource, Description = description.Trim(), Group = "Unknown" };
    }

    public void SetReservationNotes(List<Entity.LodgingReservationNote> reservationNotes, string guestNum, string noteType)
    {
        var notes = Array.Empty<ReservationNote>();
        if (reservationNotes.Count > 0)
        {
            notes = reservationNotes.Where(n => string.Equals(n.mstype, noteType, StringComparison.InvariantCultureIgnoreCase)).Select(rn => new ReservationNote { Comment = rn.mstxt.Trim(), UserId = rn.msuser, NoteCreateDate = rn.msdatetime, NoteUpdateDate = rn.msdatetime, SourceSystemKey = guestNum, NoteKey = 0 }).ToArray();
        }

        reservation.Notes = notes;
    }

    public string HeroImage(List<Reservation> reservations)
    {
        try
        {


            // need to consider event location resort detail for all activities of all reservations 
            var metaActivities = new List<ReservationActivity>();

            foreach (var res in reservations.Where(r => r.LodgingFlag))
            {
                metaActivities.AddRange(res.Activities);
            }


            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.ResortDetail) && string.Equals(a.EventLocation.ResortDetail, LodgeAtPebbleBeach)))
            {
                return "LodgingPebbleBeach";
            }

            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.ResortDetail) && string.Equals(a.EventLocation.ResortDetail, InnAtSpanishBay)))
            {
                return "LodgingSpanishBay";
            }

            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.ResortDetail) && string.Equals(a.EventLocation.ResortDetail, CasaPalmero)))
            {
                return "LodgingCasaPalmero";
            }

            metaActivities.Clear();

            foreach (var res in reservations.Where(r => r.Type == ReservationType.Golf))
            {
                metaActivities.AddRange(res.Activities);
            }


            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.Description) && a.EventLocation.Description.IndexOf(GolfPebbleBeach, 0, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                return "GolfPebbleBeach";
            }

            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.Description) && a.EventLocation.Description.IndexOf(GolfSpyglass, 0, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                return "GolfSpyglass";
            }

            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.Description) && a.EventLocation.Description.IndexOf(GolfSpanishBay, 0, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                return "GolfSpanishBay";
            }

            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.Description) && a.EventLocation.Description.IndexOf(GolfDelMonte, 0, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                return "GolfDelMonte";
            }

            if (metaActivities.Any(a => !string.IsNullOrEmpty(a?.EventLocation?.Description) && a.EventLocation.Description.IndexOf(GolfTheHay, 0, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                return "GolfTheHay";
            }


            if (reservations.Any(r => r.Type == ReservationType.FoodAndBeverage))
                return "Dining";

            return "Default";
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to determining hero image");
            return "Default";

        }

    }


    public void SetReservationActivities(AppConfig config, List<LodgingReservationTransaction> transactions)
    {
        var _reservationActivityList = new List<ReservationActivity>();

        foreach (var t in transactions)
        {

            _reservationActivityList.Add(new ReservationActivity
            {
                ActivityId = reservation.ReservationId,
                Amount = decimal.Round(t.tdebit / reservation.NumberOfDays, 2),
                Currency = config.CurrencyCode,
                Date = t.tdate,
                Quantity = (int)t.tqty,
                Status = GetActivityStatus(reservation.Status),
                EventLocation = _eventLocation,
                Product = new ReservationActivityProduct
                {
                    Code = string.IsNullOrEmpty(t.tcode) ? "RySol Generic Lodge" : t.tcode,
                    Description = string.IsNullOrEmpty(t.cdescr) ? "Generic Lodging Product" : t.cdescr,
                    LOB = "Lodging",
                    LOBSummary = "Lodging",
                },
                PurchaseLocation = _purchaseLocation,
                NameValues =
                [
                    new ReservationActivityNameValue { Name = "PostedUser", Value = t.top, },
                    new ReservationActivityNameValue { Name = "ActivityDateShort", Value = $"{t.tdate:d}" },
                    new ReservationActivityNameValue { Name = "ActivityDateLong", Value = $"{t.tdate:D}" },
                    new ReservationActivityNameValue { Name = "ActivityDateTime", Value = $"{t.tdate:h:mm tt}" },
                ],
            });

        }

        if (reservation.Status == ReservationStatus.InHouse || reservation.Status == ReservationStatus.Completed && transactions.Count == 0)
        {
            _reservationActivityList = _genericActivities;
        }
        else
        {
            _reservationActivityList.AddRange(_genericActivities);
        }

        reservation.Activities = _reservationActivityList.ToArray();
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

    public static ReservationStatus GetReservationStatus(string resStatus)
    {
        // _log.Debug("Begin: GetReservationStatus(string resStatus)");
        resStatus = resStatus.ToUpper();

        ReservationStatus status = ReservationStatus.Delete;
        if (resStatus == "NEW")
        {
            status = ReservationStatus.New;
        }
        else if (resStatus == "CNF")
        {
            status = ReservationStatus.Open;
        }
        else if (resStatus == "OOO")
        {
            status = ReservationStatus.Delete;
        }
        else if (resStatus == "INH")
        {
            status = ReservationStatus.InHouse;
        }
        else if (resStatus == "OUT")
        {
            status = ReservationStatus.Completed;
        }
        else if (resStatus == "CAN")
        {
            status = ReservationStatus.Cancelled;
        }
        else if (resStatus == @"C/L")
        {
            status = ReservationStatus.Delete;
        }
        else if (resStatus == "SHR")
        {
            status = ReservationStatus.Delete;
        }
        else
        {
            status = ReservationStatus.Delete;
        }
        //_log.Debug("End: GetReservationStatus(string resStatus)");
        return status;
    }
    public string GetCancellationId(string reservationId, DateTime cancellationDate)
    {
        // compute the SMS cancellation number
        if (reservationId.Length != 6 || !char.IsDigit(Convert.ToChar(reservationId.Substring(3, 1))) || !char.IsDigit(Convert.ToChar(reservationId.Substring(0, 1))))
        {
            return string.Empty;
        }

        var character1 = ((int)cancellationDate.DayOfWeek + 1).ToString(); // 1=sunday, 2=monday, etc.
        var character2 = reservationId.Substring(5, 1);
        var character3 = reservationId.Substring(4, 1);
        var character4 = Convert.ToChar((Convert.ToInt32(reservationId.Substring(3, 1)) + 1) + 64); // ASCII A is 65
        var character5 = reservationId.Substring(2, 1);
        var character6 = reservationId.Substring(1, 1);
        var character7 = Convert.ToChar((Convert.ToInt32(reservationId.Substring(0, 1)) + 1) + 64); // ASCII A is 65
        var cancellationId = string.Concat(character1, character2, character3, character4, character5, character6, character7);

        return cancellationId;
    }
    public (bool, string) GetPropertyName(string unit)
    {
        if (unit.ToLower().StartsWith("b"))
            return (true, InnAtSpanishBay);
        else if (unit.ToLower().StartsWith("l"))
            return (true, LodgeAtPebbleBeach);
        else if (unit.ToLower().StartsWith("p"))
            return (true, CasaPalmero);

        return (false, string.Empty);
    }

    public void SetCustomer(Customer customer)
    {
        reservation.Customer = customer;

    }
}
