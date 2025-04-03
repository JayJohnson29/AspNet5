using Sms.Entity;

namespace Sms.Repository
{
    public interface ILetterHistoryItineraryRepository
    {
        Task<List<LetterHistoryItinerary>> GetAllAsync(int smsIntegrationId);
    }
}