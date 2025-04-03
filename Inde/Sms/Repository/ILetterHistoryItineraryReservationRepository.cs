using Sms.Entity;

namespace Sms.Repository
{
    public interface ILetterHistoryItineraryReservationRepository
    {
        Task<List<LetterHistoryItineraryReservation>> GetAllAsync(LetterHistoryItinerary itinerary);
    }
}