using Sms.Entity;

namespace Sms.Repository
{
    public interface IActivityReservationRepository
    {
        Task<List<ActivityReservation>> GetAsync(string reservationId);
    }
}