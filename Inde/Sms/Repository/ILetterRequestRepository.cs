using Sms.Entity;

namespace Sms.Repository
{
    public interface ILetterRequestRepository
    {
        Task<List<LetterRequest>> CreateAsync(int smsIntegrationId, DateTime beginDate, DateTime endDate);
    }
}