using Sms.Entity;

namespace Sms.Repository
{
    public interface ILodgingReservationRatePlanPolicyRepostitory
    {
        Task<List<LodgingReservationRatePlanPolicy>> GetAsync();
    }
}