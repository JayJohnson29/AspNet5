using Sms.Entity;

namespace Sms.Adapter
{
    public interface IItineraryBuilderFactory
    {
        ItineraryBuilder Create(AppConfig config, LetterHistoryItinerary source);
    }
}