using Sms.Entity;

namespace Sms.Repository
{
    public interface IItineraryHistoryGuestRepository
    {
        Task<List<ItineraryHistoryGuest>> CreateAsync(int smsIntegrationId);
    }
}