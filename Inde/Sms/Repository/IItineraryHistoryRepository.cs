using Sms.Entity;

namespace Sms.Repository
{
    public interface IItineraryHistoryRepository
    {
        Task<List<ItineraryHistory>> CreateAsync(int smsIntegrationId, DateTime beginDate, DateTime endDate);
        Task<List<ItineraryHistoryReservationId>> GetReservationIdAsync(int smsIntegrationId);
    }
}