using Sms.Entity;

namespace Sms.Repository
{
    public interface IGuestRepository
    {
        Task<List<Guest>> GetAsync(string guestId);
    }
}