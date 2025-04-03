using Sms.Entity;

namespace Sms.Repository
{
    public interface ISmsIntegrationRepostitory
    {
        Task<List<SmsIntegration>> GetAsync();
    }
}