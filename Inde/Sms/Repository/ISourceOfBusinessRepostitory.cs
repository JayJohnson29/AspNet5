using Sms.Entity;

namespace Sms.Repository
{
    public interface ISourceOfBusinessRepostitory
    {
        Task<List<SourceOfBusiness>> GetAsync();
    }
}