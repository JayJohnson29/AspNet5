
namespace IndeService.Service
{
    public interface IMyTokenService
    {
        Task<Guid> GetAccessTokenAsync();
    }
}