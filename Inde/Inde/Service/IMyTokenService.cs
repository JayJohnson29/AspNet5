
namespace Inde.Service
{
    public interface IMyTokenService
    {
        Task<Guid> GetAccessTokenAsync();
    }
}