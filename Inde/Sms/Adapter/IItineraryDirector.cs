using Sms.Entity;

namespace Sms.Adapter
{
    public interface IItineraryDirector
    {
        Task<MC.Itinerary> RunAsync(LetterHistoryItinerary letterHistoryItinerary, AppConfig config, 
                    List<SourceOfBusiness> sourceOfBusiness,
            List<Misc> miscDescriptions,
            List<LodgingReservationRatePlanPolicy> ratePlanPolicies,
            List<User> users,
            List<LodgingUnit> unitDescriptions);
    }
}