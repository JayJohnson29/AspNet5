using Sms.Entity;

namespace Sms.Repository
{
    public interface IItineraryArrivalRepository
    {
        Task<List<ItineraryArrival>> CreateAsync(int smsIntegrationId, DateTime beginDate, DateTime endDate);
    }
}