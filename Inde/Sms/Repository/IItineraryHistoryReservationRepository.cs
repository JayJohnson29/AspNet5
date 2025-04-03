using Sms.Entity;

namespace Sms.Repository
{
    public interface IItineraryHistoryReservationRepository
    {
        Task<List<ItineraryHistoryReservation>> CreateAsync(int smsIntegrationId);
    }
}