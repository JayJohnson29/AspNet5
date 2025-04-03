using Microsoft.Extensions.Logging;
using Sms.Repository;

namespace Sms.Adapter;

public class ActivityReservationDirector : IActivityReservationDirector
{
    private readonly ILogger<ActivityReservationDirector> logger;
    private readonly IActivityReservationBuilderFactory activityReservationBuilderFactory;
    private readonly ICustomerBuilderFactory customerBuilderFactory;
    private readonly IGuestRepository guestRepository;
    private readonly IActivityReservationRepository activityReservationRepository;

    public ActivityReservationDirector(ILogger<ActivityReservationDirector> logger, IActivityReservationBuilderFactory activityReservationBuilderFactory, ICustomerBuilderFactory customerBuilderFactory, IGuestRepository guestRepository, IActivityReservationRepository activityReservationRepository)
    {
        this.logger = logger;
        this.activityReservationBuilderFactory = activityReservationBuilderFactory;
        this.customerBuilderFactory = customerBuilderFactory;
        this.guestRepository = guestRepository;
        this.activityReservationRepository = activityReservationRepository;
    }

    public async Task<MC.Reservation> Run(AppConfig config, string itineraryId, string reservationId)
    {
        var r = await activityReservationRepository.GetAsync(reservationId);
        var res = ActivityReservationCleaner.Clean(r.First());

        var r2 = activityReservationBuilderFactory.Create(config, itineraryId,res);

        var c = await guestRepository.GetAsync(res.skgnum);
        var c2 = customerBuilderFactory.Create(c.First(), config.ClientId, config.SourceSystemCode);
        r2.SetCustomer(c2.Customer );

        return r2.Reservation;
    }
}
