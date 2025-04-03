using Microsoft.Extensions.Logging;
using Serilog;
using Sms.Entity;
using Sms.MC;
using System.Runtime.CompilerServices;

namespace Sms.Adapter
{
    public class ItineraryBuilder
    {
        private readonly ILogger<ItineraryBuilder> _logger;

        private MC.Itinerary _itinerary;
        private List<MC.Reservation> _reservations = new List<MC.Reservation>();
        private List<MC.ItineraryNameValue> _itineraryNameValues = new List<MC.ItineraryNameValue>();
        private readonly string _letterNum;
        private readonly string _letterCode;

        private const string InnAtSpanishBay = "The Inn at Spanish Bay";
        private const string LodgeAtPebbleBeach = "The Lodge at Pebble Beach";
        private const string CasaPalmero = "Casa Palmero";

        private const string GolfSpanishBay = "Spanish Bay";
        private const string GolfPebbleBeach = "Pebble Beach";
        private const string GolfSpyglass = "SpyGlass";
        private const string GolfTheHay = "The Hay";
        private const string GolfDelMonte = "Del Monte";


        public MC.Itinerary Itinerary 
        { 
            get
            {
                _itinerary.Reservations = _reservations.ToArray();
                _itinerary.NameValues = _itineraryNameValues.ToArray();
                return _itinerary; 
            } 
        }


        public ItineraryBuilder(ILogger<ItineraryBuilder> logger, AppConfig config, LetterHistoryItinerary source)
        {
            _logger = logger;
            _letterCode = source.lcode;
            _letterNum = source.lnum;

            var dateVal = DateTimeUtil.CreateDateTime(source.icdate, source.ictime);
            _itinerary = new MC.Itinerary
            {
                ItineraryId = source.icode,
                CID = config.ClientId,
                SID = config.SourceSystemCode,
                CreateDate = dateVal,
                BookingDate = dateVal,
                ArrivalDate = source.iarrive,
                DepartureDate = source.idepart,
                ExpireDate = DateTimeUtil.CreateDateTime(source.iexpdate, source.iexptime),
                Totals = new MC.ItineraryTotals
                {
                    AfterTax = source.icharges,
                    Payments = source.ipayments,
                    CurrencyCode = config.CurrencyCode, // perhaps pick out of SMS
                },
                Status = "New",
            };

           
        }

        public void SetCustomer( MC.Customer customer)
        {
            _itinerary.Customer = customer;
        }

        public void SetReservations(List<MC.Reservation> reservations)
        {
           _reservations = reservations;
        }

        public bool SetAttributes()
        {
            if (_reservations.Count == 0)
            {
                //_log.Warn($"No Itinerary Components found for Itinerary {itinerary.ItineraryId}");
                return false;
            }
            //_log.Debug($"{reservations.Count} lodging reservations loaded for {itinerary.ItineraryId}");

            _itinerary.Totals.BeforeTax = _reservations.Sum(r => r.Totals.BeforeTax);
            _itinerary.Totals.AfterTax = _reservations.Sum(r => r.Totals.AfterTax);
            _itinerary.Totals.Tax = _reservations.Sum(r => r.Totals.Tax);
            _itinerary.Totals.Fees = _reservations.Sum(r => r.Totals.Fees);
            _itinerary.Totals.DepositReceived = _reservations.Sum(r => r.Totals.DepositReceived);
            _itinerary.Totals.BalanceDue = _reservations.Sum(r => r.Totals.BalanceDue);
            _itinerary.Totals.Other = _reservations.Sum(r => r.Totals.Other);
            _itinerary.Totals.Payments = _reservations.Sum(r => r.Totals.DepositReceived);

            var totalActivityPrice = CalcTotalActivityPrice();

            var lodgingReservationCount = _reservations.Count(r => r.LodgingFlag);
            var firstRes = _reservations.Min(r => r.ArrivalDate);
            var lastRes = _reservations.Max(r => r.ArrivalDate);

            if (_reservations.Any(r => r.LodgingFlag))
            {
                firstRes = _reservations.Where(r => r.LodgingFlag).Min(r => r.ArrivalDate);
                lastRes = _reservations.Where(r => r.LodgingFlag).Max(r => r.DepartureDate);
            }
            var depositRequired = CalcDepositRequiredItinerary();
            var depositAmount1Total = CalcDepositAmount1Itinerary();  // 113481 -- this is the same as depositRequired
            var depositPaid = CalcDepositDueDate();
            var grandTotal = _itinerary.Totals.AfterTax + totalActivityPrice;
            var dynamicHeroQualifier = HeroImage();
            var reservationRateTypes = GetUniqueRateTypes();
            var hasPkRateType = HasPkRateType();
            var housekeepingNoteEmailOverride = HouseKeepingEmailOverride();
            var additionalEmailRecipients = AdditionalEmailRecipients();
            var golfPackageActivityTotal = GolfPackageActivityTotal();
            var golfNonPackageActivityTotal = GolfNonPackageActivityTotal();
            var packageGrandTotal = _itinerary.Totals.AfterTax + golfNonPackageActivityTotal;  // christine asked for this 8/17/2023 -- NOTE: this is not the package grand total - its a PB specific value
            var hasGolf = _reservations.Any(r => r.Type == ReservationType.Golf);
            var hasDining = _reservations.Any(r => r.Type == ReservationType.FoodAndBeverage);
            var hasGolfOrDining = hasGolf || hasDining;                                       // for handlebar coding    
            var totalUnits = NumberOfRooms();
            var estimatedBalanceDue = CalcEstimatedBalanceDue( totalActivityPrice);
            var hasCaddyRequest = HasReservationWithCaddyRequest();
            var arrivalDaysOut = BookingToArrivalDays();
            var lessThan14Days = arrivalDaysOut != 0 && arrivalDaysOut < 14;

            if (hasPkRateType)
            {
                estimatedBalanceDue = CalcEstimatedBalanceDue(golfNonPackageActivityTotal);
            }


            if (!string.IsNullOrEmpty(housekeepingNoteEmailOverride))
            {
                _itinerary.Customer.Emails[0].EmailAddress = housekeepingNoteEmailOverride;
            }


           // _log.Debug($"Setting Itinerary Name Values");

            _itineraryNameValues = new List<ItineraryNameValue>()
            {
                    new ItineraryNameValue { Name = "LetterNum", Value = _letterNum },
                    new ItineraryNameValue { Name = "EmailLetterRequest", Value = "true" },
                    new ItineraryNameValue { Name = "LetterCode", Value = _letterCode },

                    new ItineraryNameValue { Name = "TotalRooms", Value = lodgingReservationCount.ToString() },
                    new ItineraryNameValue { Name = "FirstResArrival", Value = $"{firstRes:d}" },
                    new ItineraryNameValue { Name = "LastResArrival", Value = $"{lastRes:d}" },
                    new ItineraryNameValue { Name = "ActivityTotalPrice", Value = $"{totalActivityPrice:N}" },
                    new ItineraryNameValue { Name = "EstimatedBalanceDue", Value = $"{estimatedBalanceDue:N}" },
                    new ItineraryNameValue { Name = "DepositPaid", Value = depositPaid, },
                    new ItineraryNameValue { Name = "DepositRequired", Value = $"{depositRequired:N}", },
                    new ItineraryNameValue { Name = "GrandTotal", Value = $"{grandTotal:N}", },
                    new ItineraryNameValue { Name = "DynamicHeroQualifier" , Value = dynamicHeroQualifier},
                    new ItineraryNameValue { Name = "HasLodging" , Value = _reservations.Any(r => r.Type == ReservationType.Lodging).ToString()},
                    new ItineraryNameValue { Name = "HasGolf" , Value = _reservations.Any(r => r.Type == ReservationType.Golf).ToString()},
                    new ItineraryNameValue { Name = "HasDining" , Value = _reservations.Any( r=>r.Type == ReservationType.FoodAndBeverage).ToString()},
                    new ItineraryNameValue { Name = "HasSpa" , Value = _reservations.Any(r => r.Type == ReservationType.Spa).ToString()},
                    new ItineraryNameValue { Name = "ReservationRateTypes" , Value = reservationRateTypes },
                    new ItineraryNameValue { Name = "HasPkRateType" , Value = hasPkRateType.ToString() },

                    new ItineraryNameValue { Name = "ItineraryEmailOverride", Value = housekeepingNoteEmailOverride},
                    new ItineraryNameValue { Name = "ItineraryAdditionalEmailRecipients", Value = additionalEmailRecipients  },
                    new ItineraryNameValue { Name = "ItineraryGolfPackageActivityTotal", Value = $"{golfPackageActivityTotal:N}" },
                    new ItineraryNameValue { Name = "ItineraryGolfNonPackageActivityTotal", Value = $"{golfNonPackageActivityTotal:N}" },
                    new ItineraryNameValue { Name = "ItineraryPackageGrandTotal", Value = $"{packageGrandTotal:N}" },
                    new ItineraryNameValue { Name = "ItineraryHasGolfOrDining", Value = hasGolfOrDining.ToString() },
                    new ItineraryNameValue { Name = "ItineraryTotalUnits", Value = $"{totalUnits}" },
                    new ItineraryNameValue { Name = "ItineraryHasCaddyRequest", Value = $"{hasCaddyRequest}" },
                    new ItineraryNameValue { Name = "ItineraryDepositAmount1Total", Value = $"{depositAmount1Total:N}", },
                    new ItineraryNameValue { Name = "ItineraryFirstArrivalLessThan14Days", Value = $"{lessThan14Days}", },
            };


            UpdateReservationNameValues();
            return true;
        }

        private void UpdateReservationNameValues()
        {
            //_log.Debug($"UpdateReservationNameValues {nameValues.Count} {reservations.Count}");


            var itineraryNameValues = _itineraryNameValues.Select(itineraryNameValue => new ReservationNameValue { Name = $"{itineraryNameValue.Name}", Value = itineraryNameValue.Value });

            //_log.Debug($"Updating Reservations");
            foreach (var reservation in _reservations)
            {
                if (reservation.NameValues == null || reservation.NameValues.Length == 0)
                    continue;

                var reservationNameValues = new List<ReservationNameValue>(reservation.NameValues);

                reservationNameValues.AddRange(itineraryNameValues);
                reservation.NameValues = reservationNameValues.ToArray();
            }
        }


        public int BookingToArrivalDays()
        {
            var lodgingReservations = _reservations.Where(r => r.LodgingFlag && r.ArrivalDate > DateTime.Now.AddYears(-10)).ToList();

            var firstArrival = lodgingReservations.Count() == 0 ? new DateTime(1900, 1, 1) : lodgingReservations.Min(r => r.ArrivalDate);

            if (firstArrival < _itinerary.BookingDate)
            {
                return 0;
            }

            var days = (firstArrival - _itinerary.BookingDate).Days;
            return days;
        }

        public bool HasReservationWithCaddyRequest()
        {

            foreach (var reservation in _reservations)
            {
                //if (reservation.Type != ReservationType.Lodging)
                //    continue;

                var caddyNvs = reservation.NameValues.Where(nv => nv.Name.Equals("CaddySingle", StringComparison.OrdinalIgnoreCase) ||
                                                                  nv.Name.Equals("CaddyDouble", StringComparison.OrdinalIgnoreCase) ||
                                                                  nv.Name.Equals("CaddyFC", StringComparison.OrdinalIgnoreCase)).ToList();

                if (caddyNvs.Any(cnv => cnv.Value != string.Empty))
                    return true;
            }

            return false;
        }

        public decimal CalcEstimatedBalanceDue( decimal activityTotalPrice)
        {
            var estimatedBalanceDue = _itinerary.Totals.BeforeTax + activityTotalPrice + _itinerary.Totals.Tax + _itinerary.Totals.Fees + _itinerary.Totals.Other - _itinerary.Totals.DepositReceived;

            return estimatedBalanceDue;
        }
        public int NumberOfRooms()
        {
            var total = 0m;

            foreach (var reservation in _reservations)
            {
                if (reservation.Type != ReservationType.Lodging)
                    continue;


                var multiplier = 1m;

                var share = reservation.NameValues.FirstOrDefault(res => string.Equals(res.Name, "share", StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrEmpty(share?.Value))
                    multiplier = 0.5m;

                // in_res.units is stored in the totals TotalRoom field as well;
                //var units = reservation.NameValues.FirstOrDefault(res => string.Equals(res.Name, "units", StringComparison.CurrentCultureIgnoreCase));
                //if (units == null)
                //    continue;

                total += multiplier * reservation.Totals.TotalRooms;
            }


            return (int)Math.Round(total, MidpointRounding.AwayFromZero);
        }

        public decimal GolfNonPackageActivityTotal()
        {
            var total = 0.0m;
            foreach (var reservation in _reservations)
            {
                if (reservation.Type != ReservationType.Golf)   // exclude lodging reservations
                    continue;

                var golfRateType = reservation.NameValues.FirstOrDefault(res => string.Equals(res.Name, "golfRateType", StringComparison.CurrentCultureIgnoreCase));
                if (golfRateType != null && string.Equals(golfRateType.Value, "PK"))
                    continue;

                var priceNameValue = reservation.NameValues.FirstOrDefault(res => res.Name.ToLower() == "price");
                if (priceNameValue == null)
                    continue;

                var activityPrice = 0m;
                if (decimal.TryParse(priceNameValue.Value, out activityPrice))
                    total += activityPrice;
            }

            return total;
        }
        public decimal GolfPackageActivityTotal()
        {
            var total = 0.0m;
            foreach (var reservation in _reservations)
            {
                if (reservation.Type != ReservationType.Golf)   // exclude lodging reservations
                    continue;

                var golfRateType = reservation.NameValues.FirstOrDefault(res => string.Equals(res.Name, "golfRateType", StringComparison.CurrentCultureIgnoreCase));
                if (golfRateType == null || !string.Equals(golfRateType.Value, "PK"))
                    continue;


                var priceNameValue = reservation.NameValues.FirstOrDefault(res => string.Equals(res.Name, "price", StringComparison.CurrentCultureIgnoreCase));
                if (priceNameValue == null)
                    continue;

                var activityPrice = 0m;
                if (decimal.TryParse(priceNameValue.Value, out activityPrice))
                    total += activityPrice;
            }

            return total;
        }
        public string AdditionalEmailRecipients()
        {
            var additionalEmailRecipients = new List<string>();
            foreach (var reservation in _reservations)
            {

                if (!reservation.LodgingFlag)
                    continue;

                var nv = reservation.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "AdditionalEMailRecipients", StringComparison.CurrentCultureIgnoreCase));
                if (nv == null)
                    continue;

                // if valid deposit amount 1 value add it to deposit required
                if (!string.IsNullOrEmpty(nv.Value))
                {
                    additionalEmailRecipients.Add(nv.Value);
                }

            }

            if (additionalEmailRecipients.Count == 0)
                return string.Empty;


            return string.Join(",", additionalEmailRecipients);
        }
        public string HouseKeepingEmailOverride()
        {
            foreach (var reservation in _reservations)
            {
                // check for loding reservation 
                // if they exist add the value to deposit required
                if (!reservation.LodgingFlag)
                    continue;

                var nv = reservation.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "HousekeepingEmailOverride", StringComparison.CurrentCultureIgnoreCase));
                if (nv == null)
                    continue;

                // if valid deposit amount 1 value add it to deposit required
                if (!string.IsNullOrEmpty(nv.Value))
                {
                    return nv.Value;
                }

            }
            return string.Empty;
        }
        public string CalcDepositDueDate()
        {
            //_log.Debug($"Begin: CalcDepositDueDate Itin Id: {itin.ItineraryId} res count: {reservations.Count}");

            if (_reservations.Count == 0 || !_reservations.Any(r => r.LodgingFlag))
                return string.Empty;


            var depositAmountTotal = 0m;
            foreach (var reservation in _reservations)
            {
                var nv = reservation.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "DepositAmount1", StringComparison.CurrentCultureIgnoreCase));
                if (nv != null && !string.IsNullOrEmpty(nv.Value))
                {
                    var depositAmount = 0m;
                    decimal.TryParse(nv.Value, out depositAmount);
                    depositAmountTotal += depositAmount;
                }
            }


            if (Math.Abs(_itinerary.Totals.BalanceDue) >= Math.Abs(depositAmountTotal))
                return "Paid";


            // get first deposit due date
            var res = _reservations.FirstOrDefault(r => r.LodgingFlag);

            if (res?.NameValues == null || res.NameValues.Length == 0)
                return string.Empty;

            var nameValue = res?.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "DepositDue1", StringComparison.CurrentCultureIgnoreCase));
            if (nameValue != null && !string.IsNullOrEmpty(nameValue.Value))
                return nameValue.Value;

            return string.Empty;
        }

        public decimal CalcTotalActivityPrice()
        {
            var totalActivityPrice = 0.0m;
            foreach (var reservation in _reservations)
            {
                if (reservation.Type != ReservationType.Golf)
                    continue;


                var priceNameValue = reservation.NameValues.FirstOrDefault(res => res.Name.ToLower() == "price");
                if (priceNameValue == null)
                    continue;

                var activityPrice = 0m;
                if (decimal.TryParse(priceNameValue.Value, out activityPrice))
                    totalActivityPrice += activityPrice;
            }

            return totalActivityPrice;
        }

        public decimal CalcDepositRequiredItinerary()
        {
            var depostRequired = 0m;
            foreach (var reservation in _reservations)
            {

                // check for loding reservation and DepositAmount1 nv field
                // if they exist add the value to deposit required
                if (!reservation.LodgingFlag)
                    continue;

                var nv = reservation.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "DepositAmount1", StringComparison.CurrentCultureIgnoreCase));
                if (nv == null)
                    continue;

                // if valid deposit amount 1 value add it to deposit required
                if (decimal.TryParse(nv.Value, out var depositAmount))
                {
                    depostRequired += depositAmount;
                }

            }

            return depostRequired;
        }
        public decimal CalcDepositAmount1Itinerary()
        {
            var depositAmount1Total = 0m;
            foreach (var reservation in _reservations)
            {

                // check for loding reservation and DepositAmount1 nv field
                // if they exist add the value to deposit required
                if (!reservation.LodgingFlag)
                    continue;

                var nv = reservation.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "DepositAmount1", StringComparison.CurrentCultureIgnoreCase));
                if (nv == null)
                    continue;

                // if valid deposit amount 1 value add it to deposit required
                if (decimal.TryParse(nv.Value, out var depositAmount))
                {
                    depositAmount1Total += depositAmount;
                }

            }

            return depositAmount1Total;
        }


        public string HeroImage()
        {
            try
            {

                //_log.Debug($"HeroImage Reservation Count: {reservations.Count}");

                // need to consider event location resort detail for all activities of all reservations 
                var metaActivities = new List<ReservationActivity>();

                foreach (var res in _reservations.Where(r => r.LodgingFlag))
                {
                    metaActivities.AddRange(res.Activities);
                }

                //_log.Debug($"lodging meta activities {metaActivities.Count}");

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

                foreach (var res in _reservations.Where(r => r.Type == ReservationType.Golf))
                {
                    metaActivities.AddRange(res.Activities);
                }

                //_log.Debug($"Golf meta activities {metaActivities.Count}");

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


                if (_reservations.Any(r => r.Type == ReservationType.FoodAndBeverage))
                    return "Dining";

                //_log.Debug("using default Hero image");
                return "Default";
            }
            catch (Exception e)
            {
               // _log.Error($"Unable to determining hero image", e);
                return "Default";

            }

        }

        public string GetUniqueRateTypes()
        {
            try
            {

                //_log.Debug($"GetUniqueRateTypes");

                var uniqueTypes = new List<string>();

                foreach (var reservation in _reservations)
                {

                    if (!reservation.LodgingFlag)
                        continue;

                    var nv = reservation?.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "RateType", StringComparison.CurrentCultureIgnoreCase));
                    if (nv == null || string.IsNullOrEmpty(nv.Value))
                        continue;

                    if (!uniqueTypes.Contains(nv.Value))
                    {
                        uniqueTypes.Add(nv.Value);
                    }

                }

                if (uniqueTypes.Count == 0)
                    return string.Empty;

                var stringArray = string.Join(",", uniqueTypes);
                return stringArray;
            }
            catch (Exception e)
            {
                //_log.Error($"error setting rate types", e);
                return string.Empty;
            }

        }

        public bool HasPkRateType()
        {
            try
            {
                //_log.Debug($"HasPkRateType {reservations.Count}");

                foreach (var reservation in _reservations)
                {
                    if (!reservation.LodgingFlag)
                        continue;

                    var nv = reservation?.NameValues?.FirstOrDefault(r => string.Equals(r.Name, "RateType", StringComparison.CurrentCultureIgnoreCase));
                    if (nv == null || string.IsNullOrEmpty(nv.Value))
                        continue;

                    if (string.Equals(nv.Value, "PK", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }

                }
                return false;
            }
            catch (Exception e)
            {
                //_log.Error($"Error determining PK Rate existence", e);
                return false;
            }

        }


    }
}
