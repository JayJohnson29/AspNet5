using Sms.Entity;

namespace Sms.Repository
{
    public interface ILookupCodeRepostitory
    {
        Task<List<LookupCode>> GetAsync(string dbName);
    }
}