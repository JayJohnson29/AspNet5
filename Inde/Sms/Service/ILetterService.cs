
using MC;

namespace Sms.Service
{
    public interface ILetterService
    {
        Task<Itineraries> RunAsync(CancellationToken cancellationToken, AppConfig config);
    }
}