using Azure.Core;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using Sms.MC;
using Sms.Repository;

namespace Sms.Adapter;

public class ItineraryDirector : IItineraryDirector
{
    private readonly ILogger<ItineraryDirector> _logger;
    private readonly IItineraryBuilderFactory _itineraryBuilderFactory;
    private readonly IGuestRepository _guestRepository;
    private readonly ICustomerBuilderFactory _customerBuilderFactory;
    private readonly ILetterHistoryItineraryReservationRepository _letterHistoryItineraryReservationRepository;
    private readonly ILodgingReservationDirector _lodgingReservationDirector;
    private readonly IActivityReservationDirector _activityReservationDirector;

    public ItineraryDirector(ILogger<ItineraryDirector> logger, IItineraryBuilderFactory itineraryBuilderFactory,
                    IGuestRepository guestRepository,
                    ICustomerBuilderFactory customerBuilderFactory, 
                    ILetterHistoryItineraryReservationRepository letterHistoryItineraryReservationRepository,
                    ILodgingReservationDirector lodgingReservationDirector,
                    IActivityReservationDirector activityReservationDirector
        )
    {
        this._logger = logger;
        this._itineraryBuilderFactory = itineraryBuilderFactory;
        this._guestRepository = guestRepository;
        this._customerBuilderFactory = customerBuilderFactory;
        this._letterHistoryItineraryReservationRepository = letterHistoryItineraryReservationRepository;
        this._lodgingReservationDirector = lodgingReservationDirector;
        this._activityReservationDirector = activityReservationDirector;
    }

    public async Task<MC.Itinerary> RunAsync(LetterHistoryItinerary letterHistoryItinerary, AppConfig config, 
            List<SourceOfBusiness> sourceOfBusiness,
            List<Misc> miscDescriptions,
            List<LodgingReservationRatePlanPolicy> ratePlanPolicies,
            List<User> users,
            List<LodgingUnit> unitDescriptions
            )
    {
        var itineraryBuilder = _itineraryBuilderFactory.Create(config, letterHistoryItinerary);

        var guest = await _guestRepository.GetAsync(letterHistoryItinerary.lguestnum);
        var factory = _customerBuilderFactory.Create(guest.First(), config.ClientId, config.SourceSystemCode);

        itineraryBuilder.SetCustomer(factory.Customer);

        var reservationIds = await _letterHistoryItineraryReservationRepository.GetAllAsync(letterHistoryItinerary);

        var lodgingReservationList = new List<Reservation>();
        var activityReservationList = new List<Reservation>();

        foreach (var id in reservationIds)
        {
            if (string.Equals(id.ReservationType, "L", StringComparison.CurrentCultureIgnoreCase))
            {
                var res = await _lodgingReservationDirector.Run(config, letterHistoryItinerary.icode, id.ReservationId, sourceOfBusiness, miscDescriptions, ratePlanPolicies, users, unitDescriptions);

                if (res != null && !lodgingReservationList.Any(r => string.Equals(r.ReservationId, res.ReservationId, StringComparison.CurrentCultureIgnoreCase)))
                {
                    lodgingReservationList.Add(res);
                }
            }
            else if (string.Equals(id.ReservationType, "A", StringComparison.CurrentCultureIgnoreCase))
            {
                var res = await _activityReservationDirector.Run(config, letterHistoryItinerary.icode, id.ReservationId);
                if (res != null && !activityReservationList.Any(r => string.Equals(r.ReservationId, res.ReservationId, StringComparison.CurrentCultureIgnoreCase)))
                {
                    activityReservationList.Add(res);
                }
            }
        }

        var reservationList = new List<Reservation>();
        reservationList.AddRange(lodgingReservationList.OrderBy(r => r.ArrivalDate));
        reservationList.AddRange(activityReservationList.OrderBy(r => r.ArrivalDate));

        itineraryBuilder.SetReservations(reservationList);
        itineraryBuilder.SetAttributes();

        return itineraryBuilder.Itinerary;
    }
}
