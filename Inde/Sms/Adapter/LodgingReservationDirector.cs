using Microsoft.Extensions.Logging;
using Sms.Entity;
using Sms.MC;
using Sms.Repository;
using System.Xml.Serialization;

namespace Sms.Adapter;

public class LodgingReservationDirector : ILodgingReservationDirector
{
    private readonly ILogger<LodgingReservationDirector> _logger;
    private readonly ILodgingReservationRepository _lodgingReservationRepository;
    private readonly ISourceOfBusinessRepostitory _sourceOfBusinessRepostitory;
    private readonly ILodgingReservationNoteRepostitory _lodgingReservationNoteRepostitory;
    private readonly ILodgingReservationSpecialBillingRepostitory _lodgingReservationSpecialBillingRepostitory;
    private readonly IMiscDescriptionRepository _miscDescriptionRepository;
    private readonly ILodgingReservationRatePlanPolicyRepostitory _lodgingReservationRatePlanPolicyRepostitory;
    private readonly IUserRepostitory _userRepostitory;
    private readonly ILodgingUnitRepostitory _lodgingUnitRepostitory;
    private readonly ILodgingReservationBuilderFactory _lodgingReservationBuilderFactory;
    private readonly IGuestRepository _guestRepository;
    private readonly ICustomerBuilderFactory _customerBuilderFactory;
    private readonly ILodgingReservationTransactionRepostitory lodgingReservationTransactionRepostitory;
    private readonly ILodgingReservationTransactionBuilderFactory lodgingReservationTransactionBuilderFactory;

    public LodgingReservationDirector(ILogger<LodgingReservationDirector> logger,
        ILodgingReservationRepository lodgingReservationRepository,
        //ISourceOfBusinessRepostitory sourceOfBusinessRepostitory,
        ILodgingReservationNoteRepostitory lodgingReservationMessageRepostitory,
        ILodgingReservationSpecialBillingRepostitory lodgingReservationSpecialBillingRepostitory,
        //IMiscDescriptionRepository miscDescriptionRepository,
        //ILookupCodeRepostitory lookupCodeRepostitory,
        //ILodgingReservationRatePlanPolicyRepostitory lodgingReservationRatePlanPolicyRepostitory,
        //IUserRepostitory userRepostitory,
        //ILodgingUnitRepostitory lodgingUnitRepostitory,
        ILodgingReservationBuilderFactory lodgingReservationBuilderFactory,
        IGuestRepository guestRepository,
        ICustomerBuilderFactory customerBuilderFactory,
        ILodgingReservationTransactionRepostitory lodgingReservationTransactionRepostitory
        )
    {
        _logger = logger;
        _lodgingReservationRepository = lodgingReservationRepository;

        _lodgingReservationNoteRepostitory = lodgingReservationMessageRepostitory;
        _lodgingReservationSpecialBillingRepostitory = lodgingReservationSpecialBillingRepostitory;
        //_miscDescriptionRepository = miscDescriptionRepository;
        //_lodgingReservationRatePlanPolicyRepostitory = lodgingReservationRatePlanPolicyRepostitory;
        //_userRepostitory = userRepostitory;
        //_lodgingUnitRepostitory = lodgingUnitRepostitory;
        _lodgingReservationBuilderFactory = lodgingReservationBuilderFactory;
        _guestRepository = guestRepository;
        _customerBuilderFactory = customerBuilderFactory;
        this.lodgingReservationTransactionRepostitory = lodgingReservationTransactionRepostitory;
        this.lodgingReservationTransactionBuilderFactory = lodgingReservationTransactionBuilderFactory;
    }

    public async Task<Reservation> Run(AppConfig config, string itineraryId, string reservationId,
            List<SourceOfBusiness> sourceOfBusiness,
            List<Misc> miscDescriptions,
            List<LodgingReservationRatePlanPolicy> ratePlanPolicies,
            List<User> users,
            List<LodgingUnit> unitDescriptions)

    {
        var dbName = "pb_springermiller";

        var res1 = await _lodgingReservationRepository.GetAsync(reservationId);
        if (res1.Count() == 0 || res1.FirstOrDefault() == null)
            return new Reservation();

        var res = LodgingReservationCleaner.Clean(res1.First());


        var notes = await _lodgingReservationNoteRepostitory.GetAsync(reservationId);
        var specialBillingCodes = await _lodgingReservationSpecialBillingRepostitory.GetAsync(reservationId);
        var transactions = await this.lodgingReservationTransactionRepostitory.GetAsync(reservationId);


        var lodgingReservationBuilder = _lodgingReservationBuilderFactory.Create(res, config, itineraryId);

        lodgingReservationBuilder.SetRatePlanPolicies(ratePlanPolicies, "Dep1Policy", res.rdep1pol);
        lodgingReservationBuilder.SetRatePlanPolicies(ratePlanPolicies, "CancelPolicy", res.rcancpol);
        lodgingReservationBuilder.SetRatePlanPolicies(ratePlanPolicies, "Dep2Policy", res.rdep2pol);
        lodgingReservationBuilder.SetMarketSegment(miscDescriptions, res.mrkt);
        lodgingReservationBuilder.SetUnit(unitDescriptions, res.unit);
        lodgingReservationBuilder.SetPriceBasis(unitDescriptions, res.unitrtbase);
        lodgingReservationBuilder.SetSourceOfBusiness(sourceOfBusiness, res.rsource);
        lodgingReservationBuilder.SetLastEditUser(users, string.IsNullOrEmpty(res.lastuser) ? res.op : res.lastuser);
        lodgingReservationBuilder.SetBookingUser(users, res.op);
        lodgingReservationBuilder.SetReservationRequests(miscDescriptions, res.featrs);
        lodgingReservationBuilder.SetReservationSpecialBillingRequests(miscDescriptions, specialBillingCodes, res.Special);
        lodgingReservationBuilder.SetReservationNotes(notes, res.guestNum, "N");
        lodgingReservationBuilder.SetReservationActivities(config, transactions);




        var guest = await _guestRepository.GetAsync(res.guestNum);
        var customerFactory = _customerBuilderFactory.Create(guest.First(), config.ClientId, config.SourceSystemCode);

        lodgingReservationBuilder.SetCustomer(customerFactory.Customer);

        return lodgingReservationBuilder.Reservation;
    }

}
