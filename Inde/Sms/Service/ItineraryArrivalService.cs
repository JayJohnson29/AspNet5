﻿using Microsoft.Extensions.Logging;
using Sms.Adapter;
using MC;
using Sms.Repository;
using Inde;

namespace Sms.Service;

public class ItineraryArrivalService : IItineraryArrivalService
{
    private readonly ILogger<ItineraryArrivalService> _logger;
    private readonly IItineraryArrivalRepository _itineraryArrivalRepository;
    private readonly IItineraryHistoryRepository _itineraryHistoryRepository;
    private readonly IItineraryHistoryReservationRepository _itineraryHistoryReservationRepository;
    private readonly ILodgingReservationDirector _lodgingReservationDirector;
    private readonly IActivityReservationDirector _activityReservationDirector;
    private readonly ISourceOfBusinessRepostitory _sourceOfBusinessRepostitory;
    private readonly IMiscDescriptionRepository _miscDescriptionRepository;
    private readonly ILodgingReservationRatePlanPolicyRepostitory _lodgingReservationRatePlanPolicyRepostitory;
    private readonly IUserRepostitory _userRepostitory;
    private readonly ILodgingUnitRepostitory _lodgingUnitRepostitory;

    public ItineraryArrivalService(ILogger<ItineraryArrivalService> logger,
        IItineraryArrivalRepository itineraryArrivalRepository,
        IItineraryHistoryRepository itineraryHistoryRepository,
        IItineraryHistoryReservationRepository itineraryHistoryReservationRepository,
        IItineraryHistoryGuestRepository itineraryHistoryGuestRepository,
        ILodgingReservationRepository lodgingReservationRepository,
        IActivityReservationRepository activityReservationRepository,
        IGuestRepository guestRepository,
        ICustomerBuilderFactory customerBuilderFactory,
        ILodgingReservationDirector lodgingReservationDirector,
        IActivityReservationDirector activityReservationDirector,
        ISourceOfBusinessRepostitory sourceOfBusinessRepostitory,
        IMiscDescriptionRepository miscDescriptionRepository,
        ILodgingReservationRatePlanPolicyRepostitory lodgingReservationRatePlanPolicyRepostitory,
        IUserRepostitory userRepostitory,
        ILodgingUnitRepostitory lodgingUnitRepostitory

        )
    {
        _logger = logger;
        _itineraryArrivalRepository = itineraryArrivalRepository;
        _itineraryHistoryRepository = itineraryHistoryRepository;
        _itineraryHistoryReservationRepository = itineraryHistoryReservationRepository;
        _lodgingReservationDirector = lodgingReservationDirector;
        _activityReservationDirector = activityReservationDirector;
        _sourceOfBusinessRepostitory = sourceOfBusinessRepostitory;
        _miscDescriptionRepository = miscDescriptionRepository;
        _lodgingReservationRatePlanPolicyRepostitory = lodgingReservationRatePlanPolicyRepostitory;
        _userRepostitory = userRepostitory;
        _lodgingUnitRepostitory = lodgingUnitRepostitory;
    }

    public async Task<Reservations> Run(CancellationToken cancellationToken, AppConfig config)
    {

        var beginDate = new DateTime(2025, 3, 10);
        var endDate = new DateTime(2025, 3, 11);

        //var r = await _itineraryArrivalRepository.CreateAsync(smsIntegrationid, beginDate, endDate);

        var s = await _itineraryHistoryRepository.CreateAsync(config.SmsIntegrationId, beginDate, endDate);

        var t = await _itineraryHistoryReservationRepository.CreateAsync(config.SmsIntegrationId);

        var u = await _itineraryHistoryRepository.GetReservationIdAsync(config.SmsIntegrationId);

        var sourceOfBusiness = await _sourceOfBusinessRepostitory.GetAsync();
        var miscDescriptions = await _miscDescriptionRepository.GetAsync();
        var ratePlanPolicies = await _lodgingReservationRatePlanPolicyRepostitory.GetAsync();
        var users = await _userRepostitory.GetAsync();
        var unitDescriptions = await _lodgingUnitRepostitory.GetAsync();


        var lodgingReservationList = new List<Reservation>();
        var activityReservationList = new List<Reservation>();
        foreach (var id in u)
        {
            if (string.Equals(id.ReservationType, "L", StringComparison.CurrentCultureIgnoreCase))
            {
                var res = await _lodgingReservationDirector.Run(config,id.Icode, id.ReservationId, sourceOfBusiness, miscDescriptions, ratePlanPolicies, users, unitDescriptions);

                if (res != null && !lodgingReservationList.Any(r => string.Equals(r.ReservationId, res.ReservationId, StringComparison.CurrentCultureIgnoreCase)))
                {
                    lodgingReservationList.Add(res);
                }
            }
            else if (string.Equals(id.ReservationType, "A", StringComparison.CurrentCultureIgnoreCase))
            {
                var res = await _activityReservationDirector.Run(config,id.Icode, id.ReservationId);
                if (res != null && !activityReservationList.Any(r => string.Equals(r.ReservationId, res.ReservationId, StringComparison.CurrentCultureIgnoreCase)))
                {
                    activityReservationList.Add(res);
                }

            }
        }

        var reservationList = new List<Reservation>();
        reservationList.AddRange(lodgingReservationList);
        reservationList.AddRange(activityReservationList);

        if (reservationList.Count > 0)
        {
            var sortedList = reservationList.OrderBy(o => o.ReservationId).ToList();

            var reservations = new Reservations
            {
                ExtractionDate = DateTime.Now,
                Reservation = sortedList.ToArray(),
                NumberOfRecords = sortedList.Count
            };

            //var xmlString = SerializeObject<Reservations>(reservations);
            //File.WriteAllText($"c:\\temp\\res4a.xml", xmlString);

            //var jsonString = JsonSerializer.Serialize<Reservations>(reservations);
            //File.WriteAllText($"c:\\temp\\res4a.json", xmlString);

            // Util.SendReservations(_currentInstance.FTP, xmlString, "SMS", _currentInstance.SID);
            return reservations;
        }

        return new Reservations{ExtractionDate = DateTime.Now, BatchId = -1, NumberOfRecords = 0, Reservation = [] };

    }

}
