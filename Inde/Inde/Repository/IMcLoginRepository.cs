
namespace Inde.Repository
{
    public interface IMcLoginRepository
    {
       // Task<McLoginResponse> Login(string userId, string password);
        Task<McLoginResponse> Login();
    }
}