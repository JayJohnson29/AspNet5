using Sms.Entity;

namespace Sms.Repository
{
    public interface IMiscDescriptionRepository
    {
        Task<List<Misc>> GetAsync();
    }
}