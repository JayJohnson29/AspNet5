using Sms.Entity;

namespace Sms.Repository
{
    public interface ILodgingReservationSpecialBillingRepostitory
    {
        Task<List<LodgingReservationSpecialBilling>> GetAsync( string reservationId);
    }
}