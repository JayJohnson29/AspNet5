using Sms.Entity;

namespace Sms.Repository
{
    public interface IUserRepostitory
    {
        Task<List<User>> GetAsync();
    }
}