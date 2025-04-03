
namespace Sms.Service
{
    public interface ILetterService
    {
        Task<bool> RunAsync(CancellationToken cancellationToken, AppConfig config);
    }
}