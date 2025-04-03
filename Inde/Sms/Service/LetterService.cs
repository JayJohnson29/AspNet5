using Microsoft.Extensions.Logging;
using Sms.Adapter;
using Sms.MC;
using Sms.Repository;
using System.Xml.Serialization;
using System.Xml;

namespace Sms.Service;

public class LetterService : ILetterService
{
    private readonly ILogger<LetterService> _logger;
    private readonly ILetterRequestRepository _letterRequstRepository;
    private readonly ILetterHistoryItineraryRepository _letterHistoryItineraryRepository;
    private readonly IItineraryDirector _itineraryDirector;
    private readonly ILodgingReservationDirector _lodgingReservationDirector;
    private readonly IActivityReservationDirector _activityReservationDirector;
    private readonly ISourceOfBusinessRepostitory _sourceOfBusinessRepostitory;
    private readonly IMiscDescriptionRepository _miscDescriptionRepository;
    private readonly ILodgingReservationRatePlanPolicyRepostitory _lodgingReservationRatePlanPolicyRepostitory;
    private readonly IUserRepostitory _userRepostitory;
    private readonly ILodgingUnitRepostitory _lodgingUnitRepostitory;


    public LetterService(ILogger<LetterService> logger, ILetterRequestRepository 
        letterRequstRepository,ILetterHistoryItineraryRepository 
        letterHistoryItineraryRepository, IItineraryDirector itineraryDirector,
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
        _letterRequstRepository = letterRequstRepository;
        _letterHistoryItineraryRepository = letterHistoryItineraryRepository;
        _itineraryDirector = itineraryDirector;
        _lodgingReservationDirector = lodgingReservationDirector;
        _activityReservationDirector = activityReservationDirector;
        _sourceOfBusinessRepostitory = sourceOfBusinessRepostitory;
        _miscDescriptionRepository = miscDescriptionRepository;
        _lodgingReservationRatePlanPolicyRepostitory = lodgingReservationRatePlanPolicyRepostitory;
        _userRepostitory = userRepostitory;
        _lodgingUnitRepostitory = lodgingUnitRepostitory;

    }

    public async Task<bool> RunAsync(CancellationToken cancellationToken, AppConfig config)
    {
        //var config = new AppConfig { ClientId = 224, SourceSystemCode = 7, CurrencyCode = "USD", ResortName = "pebble" };

        var smsIntegrationid = config.SmsIntegrationId;
        var beginDate = new DateTime(2025, 3, 10);
        var endDate = new DateTime(2025, 3, 11);

        var sourceOfBusiness = await _sourceOfBusinessRepostitory.GetAsync();
        var miscDescriptions = await _miscDescriptionRepository.GetAsync();
        var ratePlanPolicies = await _lodgingReservationRatePlanPolicyRepostitory.GetAsync();
        var users = await _userRepostitory.GetAsync();
        var unitDescriptions = await _lodgingUnitRepostitory.GetAsync();


        var r = await _letterRequstRepository.CreateAsync(smsIntegrationid, beginDate, endDate);

        var requests = await _letterHistoryItineraryRepository.GetAllAsync(smsIntegrationid);

        if (requests.Count == 0)
        {
           // _logger.LogWarning ("No letter requests were loaded.  Nothing to send to MC Batch API {letterRequstId}", r.First().LetterRequestId);
            return true;
        }

        //_log.Debug($"{requests.Count} letter requests were loaded");

        //var lookupValues = GetLookupCodes();
        //// lookupValues.AddRange(GetSpecialList());

        var itineraryList = new List<MC.Itinerary>();

        foreach (var request in requests)
        {
            //    var itinerary = GetLetterRequestItinerary(request, lookupValues, smsIntegrationId);
            var itinerary = await _itineraryDirector.RunAsync(request, config, sourceOfBusiness, miscDescriptions, ratePlanPolicies, users, unitDescriptions);

            if (itinerary != null)   // && valid customer
                itineraryList.Add(itinerary);

            //    UpdateLetterHistoryRecord(request, itinerary);
        }


        //var validItineraries = itineraries.Where(itinerary => itinerary.Reservations != null && itinerary.Reservations.Length > 0).ToList();
        //return validItineraries;
        if (itineraryList.Count > 0)
        {
            List<MC.Itinerary> SortedList = itineraryList.OrderBy(o => o.ItineraryId).ToList();

            var itineraries = new Itineraries
            {
                ExtractionDate = DateTime.Now,
                Itinerary = SortedList.ToArray(),
                NumberOfRecords = SortedList.Count
            };

            var xmlString = SerializeObject<Itineraries>(itineraries);
           File.WriteAllText($"c:\\temp\\itin1a.xml", xmlString);

            //var jsonString = JsonSerializer.Serialize<Reservations>(reservations);
            //File.WriteAllText($"c:\\temp\\res4a.json", xmlString);

            // Util.SendReservations(_currentInstance.FTP, xmlString, "SMS", _currentInstance.SID);
        }


        return true;
    }

    public static string SerializeObject<T>(T dataToSerialize)
    {
        if (dataToSerialize == null)
            return string.Empty;

        var xmlserializer = new XmlSerializer(typeof(T));
        var stringWriter = new Utf8StringWriter();
        using (var writer = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
        {
            xmlserializer.Serialize(writer, dataToSerialize);
            return stringWriter.ToString();
        }
    }
}
