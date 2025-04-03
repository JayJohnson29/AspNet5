using Sms.Entity;
using MC;

namespace Sms.Adapter
{
    public interface ILodgingReservationDirector
    {
        Task<Reservation> Run(AppConfig config, string itineraryId, string reservationId,
            List<SourceOfBusiness> sourceOfBusiness,
            List<Misc> miscDescriptions,
            List<LodgingReservationRatePlanPolicy> ratePlanPolicies,
            List<User> users,
            List<LodgingUnit> unitDescriptions);
    }
}