using Sms.Entity;

namespace Sms.Repository
{
    public interface ILodgingReservationRepository
    {
        Task<List<LodgingReservation>> GetAsync(string reservationId);
    }
}