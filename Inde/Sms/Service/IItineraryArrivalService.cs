
using MC;

namespace Sms.Service
{
    public interface IItineraryArrivalService
    {
        Task<Reservations> Run(CancellationToken cancellationToken, AppConfig config);
    }
}