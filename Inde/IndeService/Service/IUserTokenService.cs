
namespace IndeService.Service
{
    public interface IUserTokenService
    {
        Task<Guid> GetAccessTokenAsync();
    }
}